using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace System.Data.Clustering
{
	public class ClusterPoint
	{
        /// <summary>
        /// Gets or sets X-coord of the point
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets Y-coord of the point
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets some additional data for point
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets cluster index
        /// </summary>
        public double ClusterIndex { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="x">X-coord</param>
        /// <param name="y">Y-coord</param>
        public ClusterPoint(double x, double y)
		{
			this.X = x;
            this.Y = y;
            this.ClusterIndex = -1;
		}

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="x">X-coord</param>
        /// <param name="y">Y-coord</param>
        public ClusterPoint(double x, double y, object tag)
        {
            this.X = x;
            this.Y = y;
            this.Tag = tag;
            this.ClusterIndex = -1;
        }
	}

}
