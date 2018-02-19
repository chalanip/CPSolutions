using CP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Dto
{
    public class UserDto
    {
        /// <summary>
        /// Get or sets the Id of the User.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the NIC of the user.
        /// </summary>
        public string NIC { get; set; }

        /// <summary>
        /// Gets or sets the town of the user.
        /// </summary>
        public string Town { get; set; }

        /// <summary>
        /// Gets or sets the contact number of the user.
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Gets or sets the active status of the user.
        /// </summary>
        public Category Category { get; set; }
    }
}
