using System.Collections.Generic;

namespace MoviesVB.Core.Data
{
    public class ResultProjection
    {
        public IList<string> Properties { get; set; }

        /// <summary>
        /// True for include and False for exclude
        /// </summary>
        public bool Include { get; set; }
    }
}
