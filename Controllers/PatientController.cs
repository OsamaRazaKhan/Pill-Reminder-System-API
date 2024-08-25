using Microsoft.Ajax.Utilities;
using Pill_Reminder_System_api24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pill_Reminder_System_api3.Controllers
{
    public class PatientController : ApiController
    {
        Pill_Reminder_SystemEntities1 db = new Pill_Reminder_SystemEntities1();

        [HttpGet]
        public HttpResponseMessage Login(string email, string password)
        {
            try
            {
                var user = db.Patients.Where(s => s.email == email && s.password == password).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");

                }
                var patient = new Patient
                {
                    fname = user.fname,
                    lname = user.lname,
                    id = user.id
                };
                return Request.CreateResponse(HttpStatusCode.OK, patient, Configuration.Formatters.JsonFormatter);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [Route("api/patient/changestate")]
        [HttpPost]
        public HttpResponseMessage ChangeState(int pid, String st)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Patients.FirstOrDefault(x => x.id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                original.state = st;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/patient/changestate2")]
        [HttpPost]
        public HttpResponseMessage ChangeState2(int pid, String st)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Patients.FirstOrDefault(x => x.id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                original.state2 = st;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMedicines(int patient_id)
        {
            Patient patient = db.Patients.Where(s => s.id == patient_id).FirstOrDefault();
            if (patient != null)
            {
                try
                {
                    var medicine = db.Medicines.Where(s => s.patient_id == patient.id);


                    if (medicine == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, medicine.Select(s => new { s.id, s.name, s.type, s.no_dosage, s.image, s.start_date, s.end_date }));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no patient");

        }

        [Route("api/Patient/getprescriptions")]
        [HttpGet]
        public HttpResponseMessage GetPrescriptions(int pid)
        {
            Patient patient = db.Patients.Where(s => s.id == pid).FirstOrDefault();
            if (patient != null)
            {
                try
                {
                    var prescription = db.Prescriptions.Where(s => s.patient_id == patient.id);


                    if (prescription == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, prescription.Select(s => new { s.id, s.med_name, s.med_type, s.no_dosage, s.Weight, s.start_date, s.end_date }));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no patient");

        }
        [Route("api/patient/getmedschedule")]
        [HttpGet]
        public HttpResponseMessage GetMedSchedule(String name, String type)
        {
            Medicine medicine = db.Medicines.Where(s => s.name == name && s.type == type).FirstOrDefault();
            if (medicine != null)
            {
                try
                {
                    var schd = db.M_Schedule.Where(s => s.med_id == medicine.id);


                    if (schd == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, schd.Select(s => new {
                        s.med_id,
                        s.mon,
                        s.tue,
                        s.wed,
                        s.thr,
                        s.fri,
                        s.sat,
                        s.sun,
                        s.morn,
                        s.noon,
                        s.even,
                        s.night
                    }));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no medicine");

        }


        [Route("api/patient/getpresschedule")]
        [HttpGet]
        public HttpResponseMessage GetPresSchedule(String name, String type)
        {
            Prescription prescription = db.Prescriptions.Where(s => s.med_name == name && s.med_type == type).FirstOrDefault();
            if (prescription != null)
            {
                try
                {
                    var schd = db.P_Schedule.Where(s => s.pres_id == prescription.id);


                    if (schd == null)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no record");

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, schd.Select(s => new {
                        s.pres_id,
                        s.mon,
                        s.tue,
                        s.wed,
                        s.thr,
                        s.fri,
                        s.sat,
                        s.sun,
                        s.morn,
                        s.noon,
                        s.even,
                        s.night
                    }));

                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "no prescription");

        }
        [Route("api/patient/getdispenser1")]
        [HttpGet]
        public HttpResponseMessage GetDispenser1(int patient_id)
        {
            var dispenser1 = db.Dispensers.Where(s => s.patient_id == patient_id).FirstOrDefault();
            if (dispenser1 != null)
            {
                var dispenser2 = new Dispenser
                {
                    disp_id = dispenser1.disp_id,
                    sun_morn = dispenser1.sun_morn,
                    sun_noon = dispenser1.sun_noon,
                    sun_even = dispenser1.sun_even,
                    sun_night = dispenser1.sun_night,
                    mon_morn = dispenser1.mon_morn,
                    mon_noon = dispenser1.mon_noon,
                    mon_even = dispenser1.mon_even,
                    mon_night = dispenser1.mon_night,
                    tue_morn = dispenser1.tue_morn,
                    tue_noon = dispenser1.tue_noon,
                    tue_even = dispenser1.tue_even,
                    tue_night = dispenser1.tue_night,
                    wed_morn = dispenser1.wed_morn,
                    wed_noon = dispenser1.wed_noon,
                    wed_even = dispenser1.wed_even,
                    wed_night = dispenser1.wed_night,
                    thr_morn = dispenser1.thr_morn,
                    thr_noon = dispenser1.thr_noon,
                    thr_even = dispenser1.thr_even,
                    thr_night = dispenser1.thr_night,
                    fri_morn = dispenser1.fri_morn,
                    fri_noon = dispenser1.fri_noon,
                    fri_even = dispenser1.fri_even,
                    fri_night = dispenser1.fri_night,
                    sat_morn = dispenser1.sat_morn,
                    sat_noon = dispenser1.sat_noon,
                    sat_even = dispenser1.sat_even,
                    sat_night = dispenser1.sat_night,

                };
                var medicines = db.Medicines.Where(m => m.patient_id == patient_id);
                String currentDate = DateTime.Now.ToString("yy-MM-dd");
                currentDate = "20" + currentDate;
                foreach (var med in medicines)
                {
                    DateTime start_date = DateTime.Parse(med.start_date);
                    DateTime end_date = DateTime.Parse(med.end_date);
                    DateTime current_date = DateTime.Parse(currentDate);
                    if (current_date < start_date || current_date > end_date)
                    {
                        ///////////////////////////
                        var m_sched = db.M_Schedule.Where(ms => ms.med_id == med.id).FirstOrDefault();
                        if (m_sched.sun == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.sun_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.sun_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.sun_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.sun_night--;
                            }
                        }
                        if (m_sched.mon == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.mon_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.mon_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.mon_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.mon_night--;
                            }
                        }
                        if (m_sched.tue == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.tue_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.tue_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.tue_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.tue_night--;
                            }
                        }
                        if (m_sched.wed == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.wed_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.wed_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.wed_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.wed_night--;
                            }
                        }
                        if (m_sched.thr == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.thr_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.thr_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.thr_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.thr_night--;
                            }
                        }
                        if (m_sched.fri == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.fri_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.fri_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.fri_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.fri_night--;
                            }
                        }
                        if (m_sched.sat == "true")
                        {
                            if (m_sched.morn == "true")
                            {
                                dispenser2.sat_morn--;
                            }
                            if (m_sched.noon == "true")
                            {
                                dispenser2.sat_noon--;
                            }
                            if (m_sched.even == "true")
                            {
                                dispenser2.sat_even--;
                            }
                            if (m_sched.night == "true")
                            {
                                dispenser2.sat_night--;
                            }
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, dispenser2);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "no dispenser");

        }
        [Route("api/patient/getdispenser2")]
        [HttpGet]
        public HttpResponseMessage GetDispenser2(int patient_id, String day, String time)
        {
            int chk = 0;
            Patient patient = db.Patients.Where(s => s.id == patient_id).FirstOrDefault();
            if (patient != null)
            {
                var disp = db.Dispensers.Where(s => s.patient_id == patient_id).FirstOrDefault();
                if (day == "sun" || day == "Sunday")
                {
                    if(time == "morn")
                    chk = (int)disp.sun_morn;

                    else if (time == "noon")
                        chk = (int)disp.sun_noon;

                    else if (time == "even")
                        chk = (int)disp.sun_even;

                    else if (time == "night")
                        chk = (int)disp.sun_night;
                }

                else if (day == "mon" || day == "Monday")
                {
                    if (time == "morn")
                        chk = (int)disp.mon_morn;

                    else if (time == "noon")
                        chk = (int)disp.mon_noon;

                    else if (time == "even")
                        chk = (int)disp.mon_even;

                    else if (time == "night")
                        chk = (int)disp.mon_night;
                }
                
                else if (day == "tue" || day == "Tuesday")
                {
                    if (time == "morn")
                        chk = (int)disp.tue_morn;

                    else if (time == "noon")
                        chk = (int)disp.tue_noon;

                    else if (time == "even")
                        chk = (int)disp.tue_even;

                    else if (time == "night")
                        chk = (int)disp.tue_night;
                }
                else if (day == "wed" || day == "Wednesday")
                {
                    if (time == "morn")
                        chk = (int)disp.wed_morn;

                    else if (time == "noon")
                        chk = (int)disp.wed_noon;

                    else if (time == "even")
                        chk = (int)disp.wed_even;

                    else if (time == "night")
                        chk = (int)disp.wed_night;
                }
                else if (day == "thr" || day == "Thursday")
                {
                    if (time == "morn")
                        chk = (int)disp.thr_morn;

                    else if (time == "noon")
                        chk = (int)disp.thr_noon;

                    else if (time == "even")
                        chk = (int)disp.thr_even;

                    else if (time == "night")
                        chk = (int)disp.thr_night;
                }
                else if (day == "fri" || day == "Friday")
                {
                    if (time == "morn")
                        chk = (int)disp.fri_morn;

                    else if (time == "noon")
                        chk = (int)disp.fri_noon;

                    else if (time == "even")
                        chk = (int)disp.fri_even;

                    else if (time == "night")
                        chk = (int)disp.fri_night;
                }

                else if (day == "sat" || day == "Saturday")
                {
                    if (time == "morn")
                        chk = (int)disp.sat_morn;

                    else if (time == "noon")
                        chk = (int)disp.sat_noon;

                    else if (time == "even")
                        chk = (int)disp.sat_even;

                    else if (time == "night")
                        chk = (int)disp.sat_night;
                }
                if(chk!=0)
                {
                    String currentDate = DateTime.Now.ToString("yy-MM-dd");
                    currentDate = "20" + currentDate;
                    DateTime current_date = DateTime.Parse(currentDate);
                    var medicines1 = db.Medicines.Where(s => s.patient_id == patient.id).ToList();
                    List<Medicine> mlist = new List<Medicine>();
                    foreach (var med in medicines1)
                    {
                        DateTime start_date = DateTime.Parse(med.start_date);
                        DateTime end_date = DateTime.Parse(med.end_date);
                        if (current_date >= start_date && current_date <= end_date)
                            mlist.Add(med);
                    }
                    if (mlist == null || mlist.Count == 0)
                    {

                        return Request.CreateResponse(HttpStatusCode.OK, "no medicine");

                    }
                    else
                    {
                        medicines1 = mlist;
                        List<int> medIds1 = medicines1.Select(m => m.id).ToList();
                        var schedules = db.M_Schedule.Where(s => medIds1.Contains((int)s.med_id)).ToList();
                        if ((day == "sun" || day == "Sunday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.sun == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "sun" || day == "Sunday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.sun == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "sun" || day == "Sunday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.sun == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "sun" || day == "Sunday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.sun == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "mon" || day == "Monday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.mon == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "mon" || day == "Monday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.mon == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "mon" || day == "Monday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.mon == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "mon" || day == "Monday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.mon == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "tue" || day == "Tuesday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.tue == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "tue" || day == "Tuesday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.tue == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "tue" || day == "Tuesday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.tue == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "tue" || day == "Tuesday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.tue == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "wed" || day == "Wednesday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.wed == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "wed" || day == "Wednesday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.wed == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "wed" || day == "Wednesday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.wed == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "wed" || day == "Wednesday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.wed == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "thr" || day == "Thursday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.thr == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "thr" || day == "Thursday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.thr == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "thr" || day == "Thursday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.thr == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "thr" || day == "Thursday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.thr == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "fri" || day == "Friday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.fri == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "fri" || day == "Friday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.fri == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "fri" || day == "Friday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.fri == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "fri" || day == "Friday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.fri == "true" && s.night == "true").ToList();
                        }
                        else if ((day == "sat" || day == "Saturday") && time == "morn")
                        {
                            schedules = schedules.Where(s => s.sat == "true" && s.morn == "true").ToList();
                        }
                        else if ((day == "sat" || day == "Saturday") && time == "noon")
                        {
                            schedules = schedules.Where(s => s.sat == "true" && s.noon == "true").ToList();
                        }
                        else if ((day == "sat" || day == "Saturday") && time == "even")
                        {
                            schedules = schedules.Where(s => s.sat == "true" && s.even == "true").ToList();
                        }
                        else if ((day == "sat" || day == "Saturday") && time == "night")
                        {
                            schedules = schedules.Where(s => s.sat == "true" && s.night == "true").ToList();
                        }

                        List<int> medIds2 = schedules.Select(s => (int)s.med_id).ToList();
                        var medicines2 = db.Medicines.Where(s => medIds2.Contains((int)s.id)).ToList();

                        return Request.CreateResponse(HttpStatusCode.OK, medicines2.Select(s => new {
                            s.id,
                            s.name,
                            s.type,
                            s.no_dosage,
                            s.image,
                            s.image_path,
                            s.color,
                            s.patient_id,
                            s.doctor_id,
                            s.caretaker_id
                        }));
                    }
                }

                else return Request.CreateResponse(HttpStatusCode.OK, "[]");
                //       return Request.CreateResponse(HttpStatusCode.OK, medicine.Select(s => new { s.id, s.name, s.type, s.no_dosage }));


            }

            return Request.CreateResponse(HttpStatusCode.OK, "no patient");

        }

        [Route("api/Patient/signup")]
        [HttpPost]
        public HttpResponseMessage Signup(Patient newuser)
        {
            try
            {
                var user = db.Patients.Where(s => s.email == newuser.email).FirstOrDefault();
                if (user != null)
                    return Request.CreateResponse(HttpStatusCode.OK, "Exsist");
                // newuser.doctor_id = 2;
                // newuser.caretaker1_id = 1;
                newuser.state = "normal";
                newuser.state2 = "normal";
                db.Patients.Add(newuser);
                db.SaveChanges();

                Dispenser ds = new Dispenser();
                ds.sun_morn = 0; ds.sun_noon = 0; ds.sun_even = 0; ds.sun_night = 0;
                ds.mon_morn = 0; ds.mon_noon = 0; ds.mon_even = 0; ds.mon_night = 0;
                ds.tue_morn = 0; ds.tue_noon = 0; ds.tue_even = 0; ds.tue_night = 0;
                ds.wed_morn = 0; ds.wed_noon = 0; ds.wed_even = 0; ds.wed_night = 0;
                ds.thr_morn = 0; ds.thr_noon = 0; ds.thr_even = 0; ds.thr_night = 0;
                ds.fri_morn = 0; ds.fri_noon = 0; ds.fri_even = 0; ds.fri_night = 0;
                ds.sat_morn = 0; ds.sat_noon = 0; ds.sat_even = 0; ds.sat_night = 0;
                ds.patient_id = newuser.id;
                db.Dispensers.Add(ds);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Created");

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/patient/adddoctor")]
        [HttpPost]
        public HttpResponseMessage AddDoctor(int pid, int did)
        {
            try
            {
                //     var original = db.Medicines.Where(s => s.id == pill.id).FirstOrDefault();
                var original = db.Patients.FirstOrDefault(p => p.id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                original.doctor_id = did;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, original.id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/patient/doctoravailability")]
        [HttpGet]
        public HttpResponseMessage DoctorAvailability(int pid)
        {
            try
            {
                var original = db.Patients.FirstOrDefault(p => p.id == pid);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "no patient");
                }


                // db.Entry(original).CurrentValues.SetValues(pill);
                if (original.doctor_id != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "true");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("api/patient/pilltaken")]
        [HttpPost]
        public HttpResponseMessage PillTaken(int patient_id, String day, String time)
        {
            try
            {
                var dispenser1 = db.Dispensers.Where(s => s.patient_id == patient_id).FirstOrDefault();

                if (day == "sun" || day == "Sunday")
                {
                    if (time == "morn")
                    {
                        dispenser1.sun_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.sun_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.sun_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.sun_night = 0;
                    }
                }
                else if (day == "mon" || day == "Monday")
                {
                    if (time == "morn")
                    {
                        dispenser1.mon_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.mon_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.mon_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.mon_night = 0;
                    }
                }
                else if (day == "tue" || day == "Tueday")
                {
                    if (time == "morn")
                    {
                        dispenser1.tue_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.tue_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.tue_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.tue_night = 0;
                    }
                }
                else if (day == "wed" || day == "Wednesday")
                {
                    if (time == "morn")
                    {
                        dispenser1.wed_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.wed_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.wed_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.wed_night = 0;
                    }
                }
                else if (day == "thr" || day == "Thursday")
                {
                    if (time == "morn")
                    {
                        dispenser1.thr_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.thr_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.thr_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.thr_night = 0;
                    }
                }
                else if (day == "fri" || day == "Friday")
                {
                    if (time == "morn")
                    {
                        dispenser1.fri_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.fri_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.fri_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.fri_night = 0;
                    }
                }
                else if (day == "sat" || day == "Saturday")
                {
                    if (time == "morn")
                    {
                        dispenser1.sat_morn = 0;
                    }
                    else if (time == "noon")
                    {
                        dispenser1.sat_noon = 0;
                    }
                    else if (time == "even")
                    {
                        dispenser1.sat_even = 0;
                    }
                    else if (time == "night")
                    {
                        dispenser1.sat_night = 0;
                    }
                }
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "deleted");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
