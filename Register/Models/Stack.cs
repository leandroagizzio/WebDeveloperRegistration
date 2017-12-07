using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Models {
    public class Stack {

        public Stack() {
            this.Developers = new HashSet<Developer>();
        }

        public int StackId { get; set; }
        public string StackName { get; set; }

        public virtual ICollection<Developer> Developers { get; set; }

    }
}