
using Pill_Reminder_System_api24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pill_Reminder_System_api3.Controllers
{
    public class DoctorController : ApiController
    {
        Pill_Reminder_SystemEntities1 db = new Pill_Reminder_SystemEntities1();

        [HttpGet]
        public HttpResponseMessage Login(string email, string password)
        {
            try
            {
                var user = db.Doctors.Where(s => s.email == email && s.password == password).FirstOrDefault();
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

        [HttpGet]
        public HttpResponseMessage GetPatients(int uid)
        {
            Doctor doctor = db.Doctors.Where(s => s.id == uid).FirstOrDefault();
            if (doctor != null)
            {
                try
                {
                    var user = db.Patients.Where(s => s.doctor_id == doctor.id);


                    if (user == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, user.Select(s => new { s.id, s.fname, s.lname/*, s.caretaker1_id */}));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no doctor");

        }

        [HttpGet]
        public HttpResponseMessage GetDoctors()
        {
            var doctor = db.Doctors;
            if (doctor != null)
            {
                try
                {

                    return Request.CreateResponse(HttpStatusCode.OK, doctor.Select(s => new { s.id, s.fname, s.lname/*, s.caretaker1_id */}));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no doctor");

        }

        [Route("api/Doctor/signup")]
        [HttpPost]
        public HttpResponseMessage Signup(Doctor newuser)
        {
            try
            {
                var user = db.Doctors.Where(s => s.email == newuser.email).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");

                db.Doctors.Add(newuser);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/Doctor/addprescription")]
        [HttpPost]
        public HttpResponseMessage addprescription(Prescription pres)
        {
            try
            {
                var pres1 = db.Prescriptions.Where(s => s.med_name == pres.med_name && s.med_type == pres.med_type && s.patient_id==pres.patient_id).FirstOrDefault();
                if (pres1 != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "already exist");
                }


                db.Prescriptions.Add(pres);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, pres.id);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [Route("api/doctor/addschedule")]
        [HttpPost]
        public HttpResponseMessage addSchedule(P_Schedule newschd)
        {
            try
            {
                var schedule = db.P_Schedule.Where(s => s.pres_id == newschd.pres_id).FirstOrDefault();
                if (schedule != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");

                db.P_Schedule.Add(newschd);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/Doctor/updatepill")]
        [HttpPost]
        public HttpResponseMessage UpdatePill(Medicine pill)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Medicines.FirstOrDefault(x => x.name == pill.name && x.type == pill.type && x.patient_id == pill.patient_id);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                if (pill.start_date != "" && pill.end_date != "")
                {
                    original.start_date = pill.start_date;
                    original.end_date = pill.end_date;
                }
                original.name = pill.name;
                original.type = pill.type;
                original.no_dosage = pill.no_dosage;
                db.SaveChanges();

                var pres = db.Prescriptions.Where(p => p.med_name == original.name && p.med_type == original.type && p.patient_id == original.patient_id).FirstOrDefault();
                if(pres != null)
                {
                    pres.no_dosage = pill.no_dosage;
                    if (pill.start_date != "" && pill.end_date != "")
                    {
                        pres.start_date = pill.start_date;
                        pres.end_date = pill.end_date;
                    }
                    db.SaveChanges();

                }
                return Request.CreateResponse(HttpStatusCode.OK, original.id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/Doctor/updateschedule")]
        [HttpPost]
        public HttpResponseMessage UpdateSchedule(M_Schedule schd)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.M_Schedule.FirstOrDefault(x => x.med_id == schd.med_id);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                else
                {
                    // db.Entry(original).CurrentValues.SetValues(pill);
                    original.mon = schd.mon;
                    original.tue = schd.tue;
                    original.wed = schd.wed;
                    original.thr = schd.thr;
                    original.fri = schd.fri;
                    original.sat = schd.sat;
                    original.sun = schd.sun;
                    original.morn = schd.morn;
                    original.noon = schd.noon;
                    original.even = schd.even;
                    original.night = schd.night;

                    db.SaveChanges();
                }
                var med = db.Medicines.Where(m => m.id == schd.med_id).FirstOrDefault();
                var pres = db.Prescriptions.Where(p => p.med_name == med.name && p.med_type == med.type && p.patient_id == med.patient_id).FirstOrDefault();
                var p_schd = db.P_Schedule.Where(ps => ps.pres_id == pres.id).FirstOrDefault();

                if(p_schd!=null)
                {
                    p_schd.mon = schd.mon;
                    p_schd.tue = schd.tue;
                    p_schd.wed = schd.wed;
                    p_schd.thr = schd.thr;
                    p_schd.fri = schd.fri;
                    p_schd.sat = schd.sat;
                    p_schd.sun = schd.sun;
                    p_schd.morn = schd.morn;
                    p_schd.noon = schd.noon;
                    p_schd.even = schd.even;
                    p_schd.night = schd.night;

                    db.SaveChanges();
                }
               

                return Request.CreateResponse(HttpStatusCode.OK, "modified");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/Doctor/deletepill")]
        [HttpPost]
        public HttpResponseMessage DeletePill(Medicine pill)
        {
            try
            {
                var original = db.Medicines.Where(s => s.name == pill.name && s.type == pill.type && s.patient_id == pill.patient_id).FirstOrDefault();
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Pill not found");
                }
                
                var schedule = db.M_Schedule.Where(s => s.med_id == original.id).FirstOrDefault();

                var prescription = db.Prescriptions.Where(ps => ps.med_name == original.name && ps.med_type == original.type && ps.patient_id == original.patient_id).FirstOrDefault();
                var p_schedule = db.P_Schedule.Where(ps => ps.pres_id == prescription.id).FirstOrDefault();

                var patient = db.Patients.Where(s => s.id == original.patient_id).FirstOrDefault();
                var dispenser = db.Dispensers.Where(s => s.patient_id == patient.id).FirstOrDefault();
                if (schedule.sun == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if(dispenser.sun_morn>0)
                        {
                             if(schedule.sun== "true" && schedule.morn=="true")               
                                dispenser.sun_morn--;
                        }
                             
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.sun_noon > 0)
                        {
                            if (schedule.sun == "true" && schedule.noon == "true")
                                dispenser.sun_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.sun_even > 0)
                        {
                            if (schedule.sun == "true" && schedule.even == "true")
                                dispenser.sun_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.sun_night > 0)
                        {
                            if (schedule.sun == "true" && schedule.night == "true")
                                dispenser.sun_night--;
                        }
                    }
                }

                if (schedule.mon == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.mon_morn > 0)
                        {
                            if (schedule.mon == "true" && schedule.morn == "true")
                                dispenser.mon_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.mon_noon > 0)
                        {
                            if (schedule.mon == "true" && schedule.noon == "true")
                                dispenser.mon_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.mon_even > 0)
                        {
                            if (schedule.mon == "true" && schedule.even == "true")
                                dispenser.mon_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.mon_night > 0)
                        {
                            if (schedule.mon == "true" && schedule.night == "true")
                                dispenser.mon_night--;
                        }
                    }
                }

                if (schedule.tue == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.tue_morn > 0)
                        {
                            if (schedule.tue == "true" && schedule.morn == "true")
                                dispenser.tue_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.tue_noon > 0)
                        {
                            if (schedule.tue == "true" && schedule.noon == "true")
                                dispenser.tue_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.tue_even > 0)
                        {
                            if (schedule.tue == "true" && schedule.even == "true")
                                dispenser.tue_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.tue_night > 0)
                        {
                            if (schedule.tue == "true" && schedule.night == "true")
                                dispenser.tue_night--;
                        }
                    }
                }

                if (schedule.wed == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.wed_morn > 0)
                        {
                            if (schedule.wed == "true" && schedule.morn == "true")
                                dispenser.wed_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.wed_noon > 0)
                        {
                            if (schedule.wed == "true" && schedule.noon == "true")
                                dispenser.wed_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.wed_even > 0)
                        {
                            if (schedule.wed == "true" && schedule.even == "true")
                                dispenser.wed_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.wed_night > 0)
                        {
                            if (schedule.wed == "true" && schedule.night == "true")
                                dispenser.wed_night--;
                        }
                    }
                }

                if (schedule.thr == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.thr_morn > 0)
                        {
                            if (schedule.thr == "true" && schedule.morn == "true")
                                dispenser.thr_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.thr_noon > 0)
                        {
                            if (schedule.thr == "true" && schedule.noon == "true")
                                dispenser.thr_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.thr_even > 0)
                        {
                            if (schedule.thr == "true" && schedule.even == "true")
                                dispenser.thr_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.thr_night > 0)
                        {
                            if (schedule.thr == "true" && schedule.night == "true")
                                dispenser.thr_night--;
                        }
                    }
                }

                if (schedule.fri == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.fri_morn > 0)
                        {
                            if (schedule.fri == "true" && schedule.morn == "true")
                                dispenser.fri_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.fri_noon > 0)
                        {
                            if (schedule.fri == "true" && schedule.noon == "true")
                                dispenser.fri_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.fri_even > 0)
                        {
                            if (schedule.fri == "true" && schedule.even == "true")
                                dispenser.fri_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.fri_night > 0)
                        {
                            if (schedule.fri == "true" && schedule.night == "true")
                                dispenser.fri_night--;
                        }
                    }
                }

                if (schedule.sat == "true")
                {
                    if (schedule.morn == "true")
                    {
                        if (dispenser.sat_morn > 0)
                        {
                            if (schedule.sat == "true" && schedule.morn == "true")
                                dispenser.sat_morn--;
                        }
                    }
                    if (schedule.noon == "true")
                    {
                        if (dispenser.sat_noon > 0)
                        {
                            if (schedule.sat == "true" && schedule.noon == "true")
                                dispenser.sat_noon--;
                        }
                    }
                    if (schedule.even == "true")
                    {
                        if (dispenser.sat_even > 0)
                        {
                            if (schedule.sat == "true" && schedule.even == "true")
                                dispenser.sat_even--;
                        }
                    }
                    if (schedule.night == "true")
                    {
                        if (dispenser.sat_night > 0)
                        {
                            if (schedule.sat == "true" && schedule.night == "true")
                                dispenser.sat_night--;
                        }
                    }
                }

                db.Entry(schedule).State = System.Data.Entity.EntityState.Deleted;
                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.Entry(prescription).State = System.Data.Entity.EntityState.Deleted;
                db.Entry(p_schedule).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "pill is deleted");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
