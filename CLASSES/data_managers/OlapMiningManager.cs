using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;

namespace WebApplication_OLAP.classes
{
    public class MiningManager
    {
        //private const string sCatalog = "Adventure Works DW 2008";
        private const string sServer = "CLARITY-7HYGMQM\\ANA";
        private const string sCatalog = "MyDataBase";
        //private const string sServer = "localhost";
        private string sResult = "Success!";

        public string SResult
        {
            get { return sResult; }
            set { sResult = value; }
        }

        /*
         * Create mining structures and models for olap
         */
        public string CreateCubeMiningStructure(string sStructName, string sAlgorithm, string sCubeName, string sDimensionName, string sKeyColumn,
            List<string> lsInputColumns, List<string> lsPredictColumns, List<string> lsMeasureInput, List<string> lsMeasurePredict)
        {
            try
            {
                // connect to cube
                Server objServer = new Server();
                objServer.Connect("DataSource=" + sServer + ";Initial Catalog=" + sCatalog);
                Database objDb = objServer.Databases[sCatalog];
                Cube objCube = objDb.Cubes[sCubeName];
                DataSourceView myView = objDb.DataSourceViews[0];

                // create mining structure
                CubeDimension objDimension = objCube.Dimensions.GetByName(sDimensionName);

                // drop the existing structures with the same name
                Microsoft.AnalysisServices.MiningStructure currentMiningStruct = objDb.MiningStructures.FindByName(sStructName);
                if (currentMiningStruct != null)
                    currentMiningStruct.Drop();

                Microsoft.AnalysisServices.MiningStructure myMiningStructure = objDb.MiningStructures.Add(sStructName, sStructName);
                myMiningStructure.Source = new CubeDimensionBinding(".", objCube.ID, objDimension.ID);


                /***************** Key column *****************/
                // key column
                CubeAttribute objKey = objCube.Dimensions.GetByName(sDimensionName).Attributes[0];
                ScalarMiningStructureColumn objKeyColumn = CreateMiningStructureColumn(objKey, true);
                objKeyColumn.Name = sKeyColumn;
                myMiningStructure.Columns.Add(objKeyColumn);


                /***************** Other columns *****************/
                // create mining columns
                for (int i = 0; i < lsInputColumns.Count; i++)
                {
                    // get attribute
                    CubeAttribute objAttribute = new CubeAttribute();
                    objAttribute = objCube.Dimensions.GetByName(sDimensionName).Attributes[0];

                    // create mining column
                    ScalarMiningStructureColumn objColumn = CreateMiningStructureColumn(objAttribute, false);
                    objColumn.Name = lsInputColumns[i];
                    myMiningStructure.Columns.Add(objColumn);


                    /***************** Measure column *****************/
                    // create mining columns for measures
                    for (int j = 0; j < lsMeasureInput.Count; j++)
                    {
                        MeasureGroup objMeasureGroup = objCube.MeasureGroups[lsMeasureInput[j]];
                        TableMiningStructureColumn objMeasure = CreateMiningStructureColumn(objMeasureGroup);

                        objMeasure.Name = lsMeasureInput[j];
                        objMeasure.Columns.Add(objColumn);
                        myMiningStructure.Columns.Add(objMeasure);
                    }
                }


                /***************** Columns for prediction - will be updated in model *****************/
                // create mining columns
                for (int i = 0; i < lsPredictColumns.Count; i++)
                {
                    // get attribute
                    CubeAttribute objAttribute = new CubeAttribute();
                    objAttribute = objCube.Dimensions.GetByName(sDimensionName).Attributes[0];

                    // create mining column
                    ScalarMiningStructureColumn objColumn = CreateMiningStructureColumn(objAttribute, false);
                    objColumn.Name = lsPredictColumns[i];
                    myMiningStructure.Columns.Add(objColumn);
                }

                // update
                myMiningStructure.Update();


                /***************** Mining model *****************/
                // create mining models
                CreateMiningModel(myMiningStructure, sStructName, sAlgorithm, lsPredictColumns, lsMeasurePredict);

                // process
                myMiningStructure.Process();

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /*
         * Create mining model
         */
        private void CreateMiningModel(Microsoft.AnalysisServices.MiningStructure objStructure, string sName, string sAlgorithm,
            List<string> lsAtrPredict, List<string> lsMeasurePredict)
        {
            Microsoft.AnalysisServices.MiningModel myMiningModel = objStructure.CreateMiningModel(true, sName);

            /* Notes:
             * Each mining column must have its' input and predict columns
             * Input and key columns are added automatically when they are created in the mining structure
             * Predict columns can be added in the mining model
             * An input column can be also a predict column
             */

            myMiningModel.Algorithm = sAlgorithm;

            switch (sAlgorithm)
            {
                case MiningModelAlgorithms.MicrosoftClustering:
                    break;
                //case MiningModelAlgorithms.MicrosoftTimeSeries:
                //    myMiningModel.AlgorithmParameters.Add("PERIODICITY_HINT", "{12}");              // {12} represents the number of months for prediction
                //    break;
                case MiningModelAlgorithms.MicrosoftNaiveBayes:
                    break;
                case MiningModelAlgorithms.MicrosoftDecisionTrees:
                    break;
            }


            /***************** Predict columns *****************/
            // add optional predict columns
            if (lsAtrPredict.Count != 0)
            {
                // predict columns
                for (int i = 0; i < lsAtrPredict.Count; i++)
                {
                    Microsoft.AnalysisServices.MiningModelColumn modelColumn = myMiningModel.Columns.GetByName(lsAtrPredict[i]);
                    modelColumn.SourceColumnID = lsAtrPredict[i];

                    // ToDo: check if this is only predict column or both predict and input
                    modelColumn.Usage = MiningModelColumnUsages.Predict;
                }
            }

            myMiningModel.Update();
        }

        /*
         * Create Scalar mining column
         */
        private ScalarMiningStructureColumn CreateMiningStructureColumn(CubeAttribute attribute, bool isKey)
        {
            ScalarMiningStructureColumn column = new
            ScalarMiningStructureColumn();
            column.Name = attribute.Attribute.Name;

            //cube attribute is usually modeled as discrete except for key column
            column.Content = (isKey ? MiningStructureColumnContents.Key : MiningStructureColumnContents.Discrete);
            column.IsKey = isKey;

            //bind column source to a cube dimension attribute
            column.Source = new CubeAttributeBinding(attribute.ParentCube.ID,
            ((CubeDimension)attribute.Parent).ID, attribute.Attribute.ID, AttributeBindingType.Name);

            //Get the column data type from the attribute key column binding.
            column.Type = MiningStructureColumnTypes.GetColumnType(attribute.Attribute.NameColumn.DataType);

            return column;
        }

        /*
         * Create table mining structure (for measuregroups)
         */
        private TableMiningStructureColumn CreateMiningStructureColumn(MeasureGroup measureGroup)
        {
            TableMiningStructureColumn column = new TableMiningStructureColumn();
            column.Name = measureGroup.Name;
            column.SourceMeasureGroup = new MeasureGroupBinding(".", ((Cube)measureGroup.Parent).ID, measureGroup.ID);

            return column;
        }

        /*
         * Return results from query
         */
        public AdomdDataReader GetQueryResult(string sQuery)
        {
            try
            {
                string sConnString = "Data Source=" + sServer + "; Initial Catalog=" + sCatalog;
                Microsoft.AnalysisServices.AdomdClient.AdomdConnection objConn = new Microsoft.AnalysisServices.AdomdClient.AdomdConnection(sConnString);
                objConn.Open();
                Microsoft.AnalysisServices.AdomdClient.AdomdCommand objCmd = objConn.CreateCommand();
                objCmd.CommandText = sQuery;

                //Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objReader = objCmd.ExecuteReader();
                //Microsoft.AnalysisServices.AdomdClient.AdomdDataAdapter objDataAdaptor = new Microsoft.AnalysisServices.AdomdClient.AdomdDataAdapter(objCmd);

                Microsoft.AnalysisServices.AdomdClient.AdomdDataReader objDataReader = objCmd.ExecuteReader(CommandBehavior.CloseConnection);

                /*
                try
                {
                    for (int i = 0; i < objDataReader.FieldCount; i++)
                    {
                        Console.Write(objDataReader.GetName(i) + "\t");
                    }
                    Console.WriteLine();
                    while (objDataReader.Read())
                    {
                        for (int i = 0; i < objDataReader.FieldCount; i++)
                        {
                            object value = objDataReader.GetValue(i);
                            string strValue = (value == null) ?
                            string.Empty : value.ToString();
                            Console.Write(strValue + "\t");
                        }
                        Console.WriteLine();
                    }
                }
                finally
                {
                    objDataReader.Close();
                }
                */

                return objDataReader;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }






        public void CreateAdomdMining()
        {
            string sConnString = "Data Source=" + sServer + "; Initial Catalog=" + sCatalog;
            Microsoft.AnalysisServices.AdomdClient.AdomdConnection olapConn = new AdomdConnection(sConnString);
            try
            {
                olapConn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            Microsoft.AnalysisServices.AdomdClient.CubeDef objCube = olapConn.Cubes["Adventure Works"];
            Microsoft.AnalysisServices.AdomdClient.Dimension myDim = objCube.Dimensions["Customer"];
            //Microsoft.AnalysisServices.MiningStructure myStructure = db.MiningStructures.Add("MarketBasket", "MarketBasket");
            //myStructure.Source = new CubeDimensionBinding(".", objCube.Name, myDim.Name);

            Microsoft.AnalysisServices.AdomdClient.Hierarchy basketAttribute;
            Microsoft.AnalysisServices.AdomdClient.Hierarchy itemAttribute;

            basketAttribute = objCube.Dimensions["Customer"].AttributeHierarchies["Customer"];
            itemAttribute = objCube.Dimensions["Product"].AttributeHierarchies["Product"];


            /* DEMO CODE

            MiningStructure ms = db.MiningStructures.Add("CubeStruct");

            string dataSourceId = ".";
            string cubeId = "Adventure Works";
            string dimensionId = "Customer";
            ms.Source = new CubeDimensionBinding(dataSourceId, cubeId, dimensionId);

            // Add a key column
            ScalarMiningStructureColumn keyColumn = ms.Columns.Add("Customer", "Customer");
            keyColumn.IsKey = true;
            keyColumn.Type = MiningStructureColumnTypes.Text;
            keyColumn.Content = MiningStructureColumnContents.Key;
            string keyAttributeId = "Customer";
            keyColumn.Source = new CubeAttributeBinding(myCube.ID, myDimension.ID, keyAttributeId, AttributeBindingType.All);

            // Add two scalar columns
            ScalarMiningStructureColumn column = ms.Columns.Add("Commute Distance", "Commute Distance");
            column.Type = MiningStructureColumnTypes.Text;
            column.Content = MiningStructureColumnContents.Discrete;
            string attributeId = "Commute Distance";
            column.Source = new CubeAttributeBinding(myCube.ID, myDimension.ID, attributeId, AttributeBindingType.All);

            column = ms.Columns.Add("Home Owner", "Home Owner");
            column.Type = MiningStructureColumnTypes.Text;
            column.Content = MiningStructureColumnContents.Discrete;
            attributeId = "Home Owner";
            column.Source = new CubeAttributeBinding(myCube.ID, myDimension.ID, attributeId, AttributeBindingType.All);
            ms.Update();

             */
        }

        public void AddMiningStructure()
        {
            Server srv = new Server();
            srv.Connect("DataSource=CLARITY-7HYGMQM\\ANA;Initial Catalog=Adventure Works DW 2008");
            Database db = srv.Databases["Adventure Works DW 2008"];
            Cube myCube = db.Cubes["Adventure Works"];

            CubeDimension myDimension = myCube.Dimensions.GetByName("Customer");
            Microsoft.AnalysisServices.MiningStructure myMiningStructure = db.MiningStructures.Add("TestMining", "TestMining");
            myMiningStructure.Source = new CubeDimensionBinding(".", myCube.ID, myDimension.ID);

            // get current mining models
            // Demo code
            foreach (Microsoft.AnalysisServices.MiningStructure ms in db.MiningStructures)
            {
                Console.WriteLine(ms.Name);

                foreach (Microsoft.AnalysisServices.MiningModel mm in ms.MiningModels)
                {
                    Console.WriteLine(mm.Name);
                }
            }

            CubeAttribute basketAttribute;
            CubeAttribute itemAttribute;
            basketAttribute = myCube.Dimensions.GetByName("Customer").Attributes[0];
            itemAttribute = myCube.Dimensions.GetByName("Product").Attributes[0];

            //basket structure column
            ScalarMiningStructureColumn basket = CreateMiningStructureColumn(basketAttribute, true);
            basket.Name = "Basket";
            myMiningStructure.Columns.Add(basket);

            //item structure column - nested table
            ScalarMiningStructureColumn item = CreateMiningStructureColumn(itemAttribute, true);
            item.Name = "Item";

            MeasureGroup measureGroup = myCube.MeasureGroups[0];
            TableMiningStructureColumn purchases = CreateMiningStructureColumn(measureGroup);

            purchases.Name = "Purchases";
            purchases.Columns.Add(item);
            myMiningStructure.Columns.Add(purchases);

            Microsoft.AnalysisServices.MiningModel myMiningModel = myMiningStructure.CreateMiningModel();
            myMiningModel.Name = "MarketBasket";
            myMiningModel.Columns["Purchases"].Usage = MiningModelColumnUsages.PredictOnly;
            myMiningModel.Algorithm = MiningModelAlgorithms.MicrosoftAssociationRules;

            try
            {
                myMiningStructure.Update(UpdateOptions.ExpandFull);
                myMiningStructure.Process(ProcessType.ProcessFull);
            }
            catch (Microsoft.AnalysisServices.OperationException e)
            {
                this.sResult = e.StackTrace;
                Console.WriteLine(e.StackTrace);
            }
        }

        // Mining sample model
        private void CreateMarketBasketModel()
        {
            CubeAttribute basketAttribute;
            CubeAttribute itemAttribute;
            Server myServer = new Server();

            myServer.Connect("DataSource=localhost;Catalog=FoodMart");
            Database myDatabase = myServer.Databases["FoodMart"];
            Cube myCube = myDatabase.Cubes["FoodMart 2000"];
            CubeDimension myDimension = myCube.Dimensions["Customer"];
            Microsoft.AnalysisServices.MiningStructure myMiningStructure =
            myDatabase.MiningStructures.Add("MarketBasket", "MarketBasket");

            myMiningStructure.Source = new CubeDimensionBinding(".", myCube.ID, myDimension.ID);
            basketAttribute = myCube.Dimensions["Customer"].Attributes["Customer"];
            itemAttribute = myCube.Dimensions["Product"].Attributes["Product"];

            //basket structure column
            ScalarMiningStructureColumn basket = CreateMiningStructureColumn(basketAttribute, true);
            basket.Name = "Basket";
            myMiningStructure.Columns.Add(basket);

            //item structure column - nested table
            ScalarMiningStructureColumn item =
            CreateMiningStructureColumn(itemAttribute, true);
            item.Name = "Item";

            MeasureGroup measureGroup = myCube.MeasureGroups[0];
            TableMiningStructureColumn purchases =
            CreateMiningStructureColumn(measureGroup);
            purchases.Name = "Purchases";
            purchases.Columns.Add(item);
            myMiningStructure.Columns.Add(purchases);

            Microsoft.AnalysisServices.MiningModel myMiningModel = myMiningStructure.CreateMiningModel();
            myMiningModel.Name = "MarketBasket";
            myMiningModel.Columns["Purchases"].Usage = MiningModelColumnUsages.PredictOnly;
            myMiningModel.Algorithm = MiningModelAlgorithms.MicrosoftAssociationRules;
        }

        public void CreateMiningModel()
        {
            //connecting the server and database
            Server myServer = new Server();
            myServer.Connect("DataSource=localhost;Catalog=FoodMart");
            Database myDatabase = myServer.Databases["FoodMart"];
            Cube myCube = myDatabase.Cubes["FoodMart 2000"];
            CubeDimension myDimension = myCube.Dimensions["Customer"];
            Microsoft.AnalysisServices.MiningStructure myMiningStructure =
            myDatabase.MiningStructures.Add("CustomerSegement", "CustomerSegement");

            //Bind the mining structure to a cube.
            myMiningStructure.Source = new CubeDimensionBinding(".",
            myCube.ID, myDimension.ID);

            // Create the key column.
            CubeAttribute customerKey = myCube.Dimensions["Customer"].Attributes["Customer"];
            ScalarMiningStructureColumn keyStructureColumn =
            CreateMiningStructureColumn(customerKey, true);
            myMiningStructure.Columns.Add(keyStructureColumn);

            //Member Card attribute
            CubeAttribute memberCard =
            myCube.Dimensions["Customer"].Attributes["Member Card"];
            ScalarMiningStructureColumn memberCardStructureColumn = CreateMiningStructureColumn(memberCard, false);
            myMiningStructure.Columns.Add(memberCardStructureColumn);

            //Total Children attribute
            CubeAttribute totalChildren = myCube.Dimensions["Customer"].Attributes["Total Children"];
            ScalarMiningStructureColumn totalChildrenStructureColumn = CreateMiningStructureColumn(totalChildren, false);
            myMiningStructure.Columns.Add(totalChildrenStructureColumn);

            //Store Sales measure ToDo: fix this!
            //Microsoft.AnalysisServices.Measure storeSales = myCube.MeasureGroups[0].Measures["Store Sales"];
            //ScalarMiningStructureColumn storeSalesStructureColumn = CreateMiningStructureColumn(storeSales, false);

            //myMiningStructure.Columns.Add(storeSalesStructureColumn);
            //Create a mining model from the mining structure. By default, all the
            //structure columns are used. Nonkey columns are with usage input
            Microsoft.AnalysisServices.MiningModel myMiningModel = myMiningStructure.CreateMiningModel(true, "CustomerSegment");

            //Set the algorithm to be clustering.
            myMiningModel.Algorithm = MiningModelAlgorithms.MicrosoftClustering;

            //Process structure and model
            try
            {
                myMiningStructure.Update(UpdateOptions.ExpandFull);
                myMiningStructure.Process(ProcessType.ProcessFull);
            }
            catch (Microsoft.AnalysisServices.OperationException e)
            {
                string err = e.Message;
            }
        }
    }
}
