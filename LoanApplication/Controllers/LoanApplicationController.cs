using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LoanApplication.Controllers
{
    public class LoanApplicationController : Controller
    {
        // GET: ContactInfo
        public ActionResult ContactInfo()
        {
            var model = (ContactInfoViewModel)GetTempData(new ContactInfoViewModel());
            return View(model);
        }

        // POST: LoanApplication/ContactInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactInfo(ContactInfoViewModel model, string btnNext)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(btnNext))
                    {
                        SetTempData(model);

                        return RedirectToAction(nameof(LoanApplication));
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }


        // GET: LoanApplication
        public ActionResult LoanApplication()
        {
            var model = (LoanApplicationViewModel)GetTempData(new LoanApplicationViewModel());
            if (HasPageReloaded(model))
            {
                return RedirectToAction(nameof(ContactInfo));
            }
            model.YearOfBirthString = model.YearOfBirth > 0 ? model.YearOfBirth.ToString() : string.Empty;
            model.Occupations = Occupations;
            model.MaritalStatuses = MaritalStatuses;

            return View(model);
        }

        // POST: LoanApplication/LoanApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanApplication(LoanApplicationViewModel model, string btnPrevious, string btnNext)
        {
            const string requiredFieldMessage = "Obligatoriskt fält";

            try
            {
                model.Occupations = Occupations;
                model.MaritalStatuses = MaritalStatuses;

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(btnNext))
                    {
                        var isValidInput = true;

                        if (!IsMaritalStatusValid(model.MaritalStatus))
                        {
                            model.MaritalStatusValidationMsg = requiredFieldMessage;
                            isValidInput = false;
                        }

                        if (!IsValidOccupation(model.Occupation))
                        {
                            model.OccupationValidationMsg = requiredFieldMessage;
                            isValidInput = false;
                        }

                        if (!IsYearOfBirthValid(model.YearOfBirthString, out int year))
                        {
                            model.YearOfBirthValidationMsg = "Ogiltigt värde";
                            isValidInput = false;
                        } 

                        if (isValidInput)
                        {
                            model.YearOfBirth = year;
                            SetTempData(model);
                            return RedirectToAction(nameof(Summary));
                        }
                    }
                    else if (!string.IsNullOrEmpty(btnPrevious))
                    {
                        if(IsYearOfBirthValid(model.YearOfBirthString, out int year))
                        {
                            model.YearOfBirth = year;
                        }
                        SetTempData(model);
                        return RedirectToAction(nameof(ContactInfo));
                    }
                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: Summary
        public ActionResult Summary()
        {
            var model = (SummaryViewModel)GetTempData(new SummaryViewModel());
            if (HasPageReloaded(model))
            {
                return RedirectToAction(nameof(ContactInfo));
            }
            model.OccupationDisplayName = OccupationDictionary[model.Occupation];
            model.MaritalStatusDisplayName = MaritalStatusDictionary[model.MaritalStatus];
            return View(model);
        }

        // POST: LoanApplication/Summary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summary(SummaryViewModel model, string btnPrevious, string btnSend)
        {
            try
            {
                model.OccupationDisplayName = OccupationDictionary[model.Occupation];
                model.MaritalStatusDisplayName = MaritalStatusDictionary[model.MaritalStatus];

                if (ModelState.IsValid)
                {
                    

                    if (!string.IsNullOrEmpty(btnSend))
                    {
                        if(IsSummaryHiddenFieldsValid(model))
                        {
                            //parameters valid, good to send
                            return View(model);
                        }                        
                    }
                    else if (!string.IsNullOrEmpty(btnPrevious))
                    {
                        SetTempData(model);
                        return RedirectToAction(nameof(LoanApplication));
                    }
                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        private static bool HasPageReloaded(LoanApplicationViewModel model)
        {
            return string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName) || string.IsNullOrEmpty(model.Email);
        }
        

        private static bool HasPageReloaded(SummaryViewModel model)
        {
            return model.Occupation == Occupation.Undefined || model.MaritalStatus == MaritalStatus.Undefined;
        }

        private static bool IsSummaryHiddenFieldsValid(SummaryViewModel model)
        {
            var isValid = true;

            if (!IsValidOccupation(model.Occupation)) isValid = false;

            if (!IsMaritalStatusValid(model.MaritalStatus)) isValid = false;

            if (!IsYearOfBirthValid(model.YearOfBirth)) isValid=false;

            return isValid;
        }

        private static bool IsValidOccupation(Occupation occupation)
        {
            if (occupation == Occupation.Undefined)
            {
                return false;
            }
            return true;
        }

        private static bool IsMaritalStatusValid(MaritalStatus maritalStatus)
        {
            if (maritalStatus == MaritalStatus.Undefined)
            {
                return false;
            }
            return true;
        }

        private static bool IsYearOfBirthValid(int year)
        {
            if (year < 1000 || year > 9999)
            {
                return false;
            }
            return true;
        }


        private bool IsYearOfBirthValid(string yearOfBirth, out int year)
        {
            if (int.TryParse(yearOfBirth, out year))
            {
                if (IsYearOfBirthValid(year)) return true;
            }
            return false;                
        }
        
        private const string ModelJson = "ModelJson"; 

        private void SetTempData(ILoanApplication loanApplication)
        {
            var modelJson = JsonConvert.SerializeObject(loanApplication);
            TempData[ModelJson] = modelJson;
        }

        private ILoanApplication GetTempData(ILoanApplication loanApplication)
        {
            if (!TryGetTempDataValue(ModelJson, out object value)) return loanApplication;

            if (!(value is string jsonstring)) return loanApplication;

            var modelJson = jsonstring;
            if (loanApplication is ContactInfoViewModel contactInfo)
            {
                loanApplication =(ContactInfoViewModel)JsonConvert.DeserializeObject(modelJson, contactInfo.GetType());
            }
            if (loanApplication is LoanApplicationViewModel loadApplication)
            {
                loanApplication = (LoanApplicationViewModel)JsonConvert.DeserializeObject(modelJson, loadApplication.GetType());
            }
            if (loanApplication is SummaryViewModel summary)
            {
                loanApplication = (SummaryViewModel)JsonConvert.DeserializeObject(modelJson, summary.GetType());
            }
            return loanApplication;
        }

        private bool TryGetTempDataValue(string key, out object res)
        {
            if (TempData.TryGetValue(key, out res))
            {
                res = TempData[key];
                return true;
            }
            return false;
        }

        private List<SelectListItem> maritalStatuses = null;

        public List<SelectListItem> MaritalStatuses
        {
            get
            {
                if (maritalStatuses == null)
                {
                    maritalStatuses = new List<SelectListItem>();
                    MaritalStatusDictionary.Keys.ToList().ForEach(a => maritalStatuses.Add(new SelectListItem(MaritalStatusDictionary[a], a.ToString())));
                }
                return maritalStatuses;
            }
        }

        protected Dictionary<Occupation, string> OccupationDictionary => new Dictionary<Occupation, string>() {
                { Occupation.Undefined, "Välj" },
                { Occupation.Unemployed, "Arbetslös" },
                { Occupation.Employed, "Fast/Tillsvidareanställd" }
        };

        protected Dictionary<MaritalStatus, string> MaritalStatusDictionary => new Dictionary<MaritalStatus, string>()
            {
                { MaritalStatus.Undefined, "Välj" },
                { MaritalStatus.Married, "Gift" },
                { MaritalStatus.Unmarried, "Ogift" },
                { MaritalStatus.SwedishSambo, "Sambo" },
                { MaritalStatus.Divorced, "Skiljd" },                
                { MaritalStatus.Widow, "Änka" }
            };

        private List<SelectListItem> occupations = null;

        private List<SelectListItem> Occupations
        {
            get
            {
                if (occupations == null)
                {
                    occupations = new List<SelectListItem>();
                    OccupationDictionary.Keys.ToList().ForEach(a => occupations.Add(new SelectListItem(OccupationDictionary[a], a.ToString())));
                }
                return occupations;
            }
        }
    }
}