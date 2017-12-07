using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Models {
    public class Technology {

        public Technology() {
            this.Developers = new HashSet<Developer>();
        }

        public int TechnologyId { get; set; }
        public string TechnologyName { get; set; }

        public virtual ICollection<Developer> Developers { get; set; }

    }
}