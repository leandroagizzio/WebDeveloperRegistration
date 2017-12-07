using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Register.Models {
    public partial class Developer {

        public Developer() {
            this.Stacks = new HashSet<Stack>();
            this.Technologies = new HashSet<Technology>();
        }

        public int DeveloperId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DayOfBirth { get; set; }
        public int YearsExperience { get; set; }
        public string Comments { get; set; }
        public string FullName {
            get {
                return LastName + ", " + FirstName;
            }
        }

        public virtual ICollection<Technology> Technologies { get; set; }
        public virtual ICollection<Stack> Stacks { get; set; }

        public string Techs {
            get {
                string techs = "";
                foreach (Technology t in Technologies) {
                    techs += t.TechnologyName + ", ";
                }
                return techs;
            }
        }

        public string Stacs {
            get {
                string stacs = "";
                foreach (Stack s in Stacks) {
                    stacs += s.StackName + ", ";
                }
                return stacs;
            }
        }



    }
}