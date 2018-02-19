using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace service.authorise.contracts
{
    public class UserSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Token { get; set; }

        public int UserDataId { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public DateTime Created { get; set; } = DateTime.Now;

        public virtual UserData User { get; set; }
    }
}
