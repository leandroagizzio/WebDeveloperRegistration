using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Register.DAL;
using Register.Models;
using Register.ViewModel;

namespace Register.Controllers
{
    public class DevelopersController : Controller
    {
        private RegisterDBContext db = new RegisterDBContext();

        // GET: Developers
        public ActionResult Index()
        {            
            return View(db.Developers.ToList());
        }

        // GET: Developers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.Developers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }
            return View(developer);
        }

        // GET: Developers/Create
        public ActionResult Create() {
            
            //load technologies
            ViewBag.Technologies = loadTechnologies();

            //load stacks
            ViewBag.Stacks = loadStacks();

            return View();
        }

        private List<AssignedData> loadTechnologies() {
            return db.Technologies.Select(x => new AssignedData {
                Id = x.TechnologyId,
                Name = x.TechnologyName,
                Assigned = false
            }).ToList<AssignedData>();
        }

        private List<AssignedData> loadStacks() {
            return db.Stacks.Select(x => new AssignedData {
                Id = x.StackId,
                Name = x.StackName,
                Assigned = false
            }).ToList<AssignedData>();
        }

        // POST: Developers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeveloperId,FirstName,LastName,Address,Email,Phone,DayOfBirth,YearsExperience,Comments")] Developer developer, string[] selectedTechs, string[] selectedStacks) {
            if (ModelState.IsValid) {
                /* old 
                // what was selected
                var selectedTechsHS = new HashSet<string>(selectedTechs);
                // iterate through saved technologies
                foreach (var techs in db.Technologies) {
                    // if tech selected exists
                    if (selectedTechs.Contains(techs.TechnologyId.ToString())) {
                        developer.Technologies.Add(techs);
                    }
                }*/
                // old version
                // call function <db> (selected input, db, developer)
                //getSelected<Technology>(selectedTechs, db.Technologies, developer);
                //getSelected<Stack>(selectedStacks, db.Stacks, developer);

                getSelected(selectedTechs, db.Technologies, db.Technologies.Select(x => x.TechnologyId).ToList(), developer.Technologies);
                getSelected(selectedStacks, db.Stacks, db.Stacks.Select(x => x.StackId).ToList(), developer.Stacks);
                
                db.Developers.Add(developer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(developer);
        }

        // new version
        // selected items, db, list of ids in db, collection to be added items
        private void getSelected<T>(string[] sel, DbSet<T> db, List<int> dbList, ICollection<T> developer)
            where T : class {
            // none selected
            if (sel == null) {
                return; 
            }
            var selected = new HashSet<string>(sel);
            foreach (var dbItem in dbList) {
                if (selected.Contains(dbItem.ToString())) {
                    developer.Add(db.Find(dbItem));                    
                }
            }
        }

        /*old
         * problem is that is static and need implementation for any new item
         * public void getSelected<T>(string[] sel, DbSet<T> db, Developer developer)
            where T : class {
            var selected = new HashSet<string>(sel);
            foreach (var sels in db) {
                if ((new Technology()).GetType() == typeof(T)) {
                    if (selected.Contains(((Technology) (Object)sels).TechnologyId.ToString())) {
                        developer.Technologies.Add((Technology)(Object)sels);
                    }
                }
                if ((new Stack()).GetType() == typeof(T)) {
                    if (selected.Contains(((Stack)(Object)sels).StackId.ToString())) {
                        developer.Stacks.Add((Stack)(Object)sels);
                    }
                }
            }
        }*/

        // GET: Developers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Developer developer = db.Developers.Find(id);
            Developer developer = db.Developers
                .Include(d => d.Technologies)
                .Include(d => d.Stacks)
                .Where(d => d.DeveloperId == id)
                .Single();
            //AssignedTechs(developer);
            
            ViewBag.Technologies = getAssignedData(developer.Technologies
                .Select(x => x.TechnologyId).ToList(), loadTechnologies());
            ViewBag.Stacks = getAssignedData(developer.Stacks
                .Select(x => x.StackId).ToList(), loadStacks());

            if (developer == null) {
                return HttpNotFound();
            }
            return View(developer);
        }

        private List<AssignedData> getAssignedData(List<int> devList, List<AssignedData> dbList){
            var retorno = new List<AssignedData>();
            foreach (var dbItem in dbList) {
                retorno.Add(new AssignedData {
                    Id = dbItem.Id,
                    Name = dbItem.Name,
                    Assigned = devList.Contains(dbItem.Id)
                });
            }
            return retorno;
        }
        /* old
         * 
        private void AssignedTechs(Developer developer) {
            var allTechs = db.Technologies;
            var devTechs = new HashSet<int>(developer.Technologies.Select(x => x.TechnologyId));
            var viewModel = new List<AssignedTechnologyData>();
            foreach (var techs in allTechs) {
                viewModel.Add(new AssignedTechnologyData {
                    TechnologyId = techs.TechnologyId,
                    TechName = techs.TechnologyName,
                    Assigned = devTechs.Contains(techs.TechnologyId)
                } );
            }
            ViewBag.Technologies = viewModel;
        }*/

        // POST: Developers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "DeveloperId,FirstName,LastName,Address,Email,Phone,DayOfBirth,YearsExperience,Comments")] Developer developer, string[] selectedTechs) {
        public ActionResult Edit(int? id, string[] selectedTechs, string[] selectedStacks) {
            Developer devToUpdate = null; ;
            if (ModelState.IsValid) {

                devToUpdate = db.Developers
                .Include(d => d.Technologies)
                .Include(d => d.Stacks)
                .Where(d => d.DeveloperId == id)
                .Single();

                TryUpdateModel(devToUpdate, "",
                    new string[] {
                    "FirstName", "LastName", "Address", "Email", "Phone", "DayOfBirth", "YearsExperience"
                    , "Comments" });

                //old
                //UpdateDevTechs(selectedTechs, devToUpdate);
                UpdateDev<Technology>(db.Technologies, devToUpdate.Technologies, 
                    (x => x.TechnologyId), selectedTechs);

                UpdateDev<Stack>(db.Stacks, devToUpdate.Stacks,
                    (x => x.StackId), selectedStacks);
                
                
                db.Entry(devToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(devToUpdate);
        }
        
        private void UpdateDev<T>(DbSet<T> db, ICollection<T> developer, Func<T, int> condition, string[] sel)
            where T : class{
            // none selected
            if (sel == null) {
                //developer = new List<T>();
                developer.Clear();
                return;
            }

            var devList = developer.Select(condition).ToList();
            var dbList = db.Select(condition).ToList();
            // selected in page
            //var selected = new HashSet<string>(sel);
            foreach (var dbItem in dbList) {
                //if tech was selected
                if (sel.Contains(dbItem.ToString())) {
                    // if tech selected is not active on db
                    if (!devList.Contains(dbItem)) {
                        developer.Add(db.Find(dbItem));
                    }
                } else { // if tech was not selected
                    // if tech not selected is active on db
                    if (devList.Contains(dbItem)) {
                        developer.Remove(db.Find(dbItem));
                    }
                }
            }
        }

        /* old
        private void UpdateDevTechs(string[] selectedTechs, Developer devToUpdate) {
            // if none selected, create new empty
            if (selectedTechs == null) {
                devToUpdate.Technologies = new List<Technology>();
                return;
            }
            // what was selected in webpage
            var selectedTechsHS = new HashSet<string>(selectedTechs);
            // what is selected in db
            var devTechs = new HashSet<int>(devToUpdate.Technologies.Select(x => x.TechnologyId));

            foreach (var techs in db.Technologies) {
                // if tech was selected
                if (selectedTechs.Contains(techs.TechnologyId.ToString())) {
                    // if tech selected is not active on db
                    if (!devTechs.Contains(techs.TechnologyId)) {
                        devToUpdate.Technologies.Add(techs);
                    }
                } else { // if tech not selected
                    // if tech not selected is active on db
                    if (devTechs.Contains(techs.TechnologyId)) {
                        devToUpdate.Technologies.Remove(techs);
                    }
                }
            }

        }*/

        // GET: Developers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.Developers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }
            return View(developer);
        }

        // POST: Developers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Developer developer = db.Developers.Find(id);
            db.Developers.Remove(developer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
