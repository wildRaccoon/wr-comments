using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace service.authorise.contracts
{
    public class UserData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserDataId { get; set; }

        public string UserIdentity { get; set; }

        public string Password { get; set; }

        public virtual UserSession Session { get; set; }
    }
}
