using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataUtility
{
    /// <summary>
    /// Define connection details.
    /// </summary>
    public class ConnectionDto
    {
        /// <summary>
        /// Number of the connection.
        /// </summary>
        public string ConnectionName { get; set; }
                
        /// <summary>
        /// Parameter list to make connection string.
        /// </summary>
        public List<KeyValuePair<string, string>> Properties { get; set; }

        /// <summary>
        /// Connection string to database.
        /// </summary>
        public string ConnectionString { get; set; }

    }
}
