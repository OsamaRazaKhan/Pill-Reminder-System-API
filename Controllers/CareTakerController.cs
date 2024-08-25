using Pill_Reminder_System_api24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Pill_Reminder_System_api3.Controllers
{
    public class CareTakerController : ApiController
    {
        Pill_Reminder_SystemEntities1 db = new Pill_Reminder_SystemEntities1();

        [HttpGet]
        public HttpResponseMessage Login(string email, string password)
        {
            try
            {
                var user = db.CareTakers.Where(s => s.email == email && s.password == password).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");

                }
                return Request.CreateResponse(HttpStatusCode.OK, user.id);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        public HttpResponseMessage GetPatients(int uid)
        {
            CareTaker caretaker = db.CareTakers.Where(s => s.id == uid).FirstOrDefault();
            if (caretaker != null)
            {
                try
                {
                    var patie_caret = db.Patient_CareTaker.Where(s => s.caretaker_id == caretaker.id);
                    List<int?> patieIds1 = patie_caret.Select(pc => pc.patient_id).ToList();
                    var user = db.Patients.Where(s => patieIds1.Contains((int)s.id)).ToList();

                    if (user == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, user.Select(s => new { s.id, s.fname, s.lname, s.doctor_id }));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no caretaker");

        }
        [HttpGet]
        public HttpResponseMessage GetCareTakers()
        {
            var caretaker = db.CareTakers;
            if (caretaker != null)
            {
                try
                {

                    return Request.CreateResponse(HttpStatusCode.OK, caretaker.Select(s => new { s.id, s.fname, s.lname/*, s.caretaker1_id */}));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no caretaker");

        }
        [Route("api/CareTaker/signup")]
        [HttpPost]
        public HttpResponseMessage Signup(CareTaker newuser)
        {
            try
            {
                var user = db.CareTakers.Where(s => s.email == newuser.email).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");

                db.CareTakers.Add(newuser);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/caretaker/addpill")]
        [HttpPost]
        public HttpResponseMessage addpill(Medicine newpill)
        {
            try
            {

                var pill = db.Medicines.Where(s => s.name == newpill.name && s.type == newpill.type && s.patient_id == newpill.patient_id).FirstOrDefault();
                if (pill == null)
                {
                    if (newpill.color.Contains("MaterialColor"))
                    {
                        var regex = new Regex(@"Color\((0x[0-9a-fA-F]+)\)");
                        var match = regex.Match(newpill.color);
                        if (match.Success)
                        {
                            newpill.color = $"Color({match.Groups[1].Value})"; // Extracted and formatted as Color(0xff2196f3)
                        }
                    }

                    if (newpill.caretaker_id == 0)
                        newpill.caretaker_id = null;
                    else if (newpill.doctor_id == 0)
                        newpill.doctor_id = null;

                    db.Medicines.Add(newpill);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, newpill.id);

                }
                return Request.CreateResponse(HttpStatusCode.OK, pill.id);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
            }

            /* catch (Exception ex)
             {
                 return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
             }*/

        }
        [Route("api/caretaker/deletepres")]
        [HttpDelete]
        public HttpResponseMessage deletePres(String med_name, String med_type, int patient_id)
        {
            var pres = db.Prescriptions.Where(s => s.med_name == med_name && s.med_type == med_type && s.patient_id == patient_id).FirstOrDefault();
            if (pres == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "not found");
            }
            var p_shcedule = db.P_Schedule.Where(s => s.pres_id == pres.id).FirstOrDefault();
            db.Entry(p_shcedule).State = System.Data.Entity.EntityState.Deleted;
            db.Entry(pres).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "deleted");

        }
            [Route("api/caretaker/addobject")]
        [HttpPost]
        public HttpResponseMessage addobject(Patient_CareTaker p_c)
        {
            try
            {
                var obj = db.Patient_CareTaker.Where(s => s.patient_id == p_c.patient_id && s.caretaker_id == p_c.caretaker_id).FirstOrDefault();
                if (obj != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");

                int count = db.Patient_CareTaker.Where(s => s.patient_id == p_c.patient_id).Count();
                if (count == 0)
                    p_c.priority = 1;
                else if (count == 1)
                    p_c.priority = 2;
                else if (count == 2)
                    p_c.priority = 3;

                db.Patient_CareTaker.Add(p_c);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, p_c.id);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckPriority(int cid, int pid)
        {
            try
            {
                var user = db.Patient_CareTaker.Where(s => s.caretaker_id == cid && s.patient_id == pid).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");

                }
                return Request.CreateResponse(HttpStatusCode.OK, user.priority);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [Route("api/caretaker/CheckStatus")]
        [HttpGet]
        public HttpResponseMessage CheckStatus(int cid, int pid)
        {
            try
            {
                var obj = db.Patient_CareTaker.Where(s => s.caretaker_id == cid && s.patient_id == pid).FirstOrDefault();
                if (obj == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");

                }

                if (obj.priority == 1)
                {
                    var patient = db.Patients.Where(s => s.id == pid).FirstOrDefault();
                    if (patient.state == "emergency")
                    {
                        patient.state = "normal";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }

                else if (obj.priority == 2)
                {
                    var obj2 = db.Patient_CareTaker.Where(s => s.patient_id == pid && s.priority == 1).FirstOrDefault();
                    String chk = obj2.status;
                    if (obj2.status == "busy")
                    {
                        obj2.status = "available";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }
                else if (obj.priority == 3)
                {
                    var obj2 = db.Patient_CareTaker.Where(s => s.patient_id == pid && s.priority == 2).FirstOrDefault();
                    String chk = obj2.status;
                    if (obj2.status == "busy")
                    {
                        obj2.status = "available";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("api/caretaker/CheckStatus2")]
        [HttpGet]
        public HttpResponseMessage CheckStatus2(int cid, int pid)
        {
            try
            {
                var obj = db.Patient_CareTaker.Where(s => s.caretaker_id == cid && s.patient_id == pid).FirstOrDefault();
                if (obj == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");

                }

                if (obj.priority == 1)
                {
                    var patient = db.Patients.Where(s => s.id == pid).FirstOrDefault();
                    if (patient.state2 == "busy")
                    {
                        patient.state2 = "normal";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }

                else if (obj.priority == 2)
                {
                    var obj2 = db.Patient_CareTaker.Where(s => s.patient_id == pid && s.priority == 1).FirstOrDefault();
                    String chk = obj2.status2;
                    if (obj2.status2 == "busy")
                    {
                        obj2.status2 = "available";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }
                else if (obj.priority == 3)
                {
                    var obj2 = db.Patient_CareTaker.Where(s => s.patient_id == pid && s.priority == 2).FirstOrDefault();
                    String chk = obj2.status2;
                    if (obj2.status2 == "busy")
                    {
                        obj2.status2 = "available";
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "busy");
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "available");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Route("api/CareTaker/changestatus")]
        [HttpPost]
        public HttpResponseMessage ChangeStatus(int cid, int pid)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Patient_CareTaker.FirstOrDefault(x => x.caretaker_id == cid && x.patient_id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                original.status = "busy";
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/CareTaker/changestatus2")]
        [HttpPost]
        public HttpResponseMessage ChangeStatus2(int cid, int pid)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Patient_CareTaker.FirstOrDefault(x => x.caretaker_id == cid && x.patient_id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                original.status2 = "busy";
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/CareTaker/addschedule")]
        [HttpPost]
        public HttpResponseMessage addSchedule(M_Schedule newschd)
        {
            try
            {
                var schedule = db.M_Schedule.Where(s => s.med_id == newschd.med_id).FirstOrDefault();
                if (schedule != null)
                {
                    schedule.mon = newschd.mon;
                    schedule.tue = newschd.tue;
                    schedule.wed = newschd.wed;
                    schedule.thr = newschd.thr;
                    schedule.fri = newschd.fri;
                    schedule.sat = newschd.sat;
                    schedule.sun = newschd.sun;
                    schedule.morn = newschd.morn;
                    schedule.even = newschd.even;
                    schedule.noon = newschd.noon;
                    schedule.night = newschd.night;
                    db.SaveChanges();
                }

                else
                {
                    db.M_Schedule.Add(newschd);
                    db.SaveChanges();
                }
                

                var medicine = db.Medicines.Where(s => s.id == newschd.med_id).FirstOrDefault();
                var patient = db.Patients.Where(s => s.id == medicine.patient_id).FirstOrDefault();
                var dispenser = db.Dispensers.Where(s => s.patient_id == patient.id).FirstOrDefault();

                List<int> medIds = db.Medicines.Where(m=>m.patient_id==patient.id).Select(m=> m.id).ToList();
                var m_schedules = db.M_Schedule.Where(s => medIds.Contains((int)s.med_id)).ToList();

                if (newschd.sun == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.sun == "true" && s.morn == "true").Count();
                        dispenser.sun_morn  = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.sun == "true" && s.noon == "true").Count();
                        dispenser.sun_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.sun == "true" && s.even == "true").Count();
                        dispenser.sun_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.sun == "true" && s.night == "true").Count();
                        dispenser.sun_night = count;
                    }
                }

                if (newschd.mon == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.mon == "true" && s.morn == "true").Count();
                        dispenser.mon_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.mon == "true" && s.noon == "true").Count();
                        dispenser.mon_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.mon == "true" && s.even == "true").Count();
                        dispenser.mon_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.mon == "true" && s.night == "true").Count();
                        dispenser.mon_night = count;
                    }
                }

                if (newschd.tue == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.tue == "true" && s.morn == "true").Count();
                        dispenser.tue_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.tue == "true" && s.noon == "true").Count();
                        dispenser.tue_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.tue == "true" && s.even == "true").Count();
                        dispenser.tue_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.tue == "true" && s.night == "true").Count();
                        dispenser.tue_night = count;
                    }
                }

                if (newschd.wed == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.wed == "true" && s.morn == "true").Count();
                        dispenser.wed_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.wed == "true" && s.noon == "true").Count();
                        dispenser.wed_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.wed == "true" && s.even == "true").Count();
                        dispenser.wed_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.wed == "true" && s.night == "true").Count();
                        dispenser.wed_night = count;
                    }
                }

                if (newschd.thr == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.thr == "true" && s.morn == "true").Count();
                        dispenser.thr_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.thr == "true" && s.noon == "true").Count();
                        dispenser.thr_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.thr == "true" && s.even == "true").Count();
                        dispenser.thr_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.thr == "true" && s.night == "true").Count();
                        dispenser.thr_night = count;
                    }
                }

                if (newschd.fri == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.fri == "true" && s.morn == "true").Count();
                        dispenser.fri_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.fri == "true" && s.noon == "true").Count();
                        dispenser.fri_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.fri == "true" && s.even == "true").Count();
                        dispenser.fri_even= count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.fri == "true" && s.night == "true").Count();
                        dispenser.fri_night = count;
                    }
                }

                if (newschd.sat == "true")
                {
                    if (newschd.morn == "true")
                    {
                        int count = m_schedules.Where(s => s.sat == "true" && s.morn == "true").Count();
                        dispenser.sat_morn = count;
                    }
                    if (newschd.noon == "true")
                    {
                        int count = m_schedules.Where(s => s.sat == "true" && s.noon == "true").Count();
                        dispenser.sat_noon = count;
                    }
                    if (newschd.even == "true")
                    {
                        int count = m_schedules.Where(s => s.sat == "true" && s.even == "true").Count();
                        dispenser.sat_even = count;
                    }
                    if (newschd.night == "true")
                    {
                        int count = m_schedules.Where(s => s.sat == "true" && s.night == "true").Count();
                        dispenser.sat_night = count;
                    }
                }
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
