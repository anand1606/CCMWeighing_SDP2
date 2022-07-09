using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;

namespace CCMDataCapture
{
    class clsMachine
    {
        private zkemkeeper.CZKEM CZKEM1;
        
#pragma warning disable CS0169 // The field 'clsMachine._location' is never used
#pragma warning disable CS0169 // The field 'clsMachine._machinedesc' is never used
        private string _ip,_machinedesc , _tableName,_location;
#pragma warning restore CS0169 // The field 'clsMachine._machinedesc' is never used
#pragma warning restore CS0169 // The field 'clsMachine._location' is never used
        private bool _connected;
#pragma warning disable CS0169 // The field 'clsMachine._fingerprintversion' is never used
#pragma warning disable CS0414 // The field 'clsMachine._version' is assigned but its value is never used
        private string _version,_fingerprintversion;
#pragma warning restore CS0414 // The field 'clsMachine._version' is assigned but its value is never used
#pragma warning restore CS0169 // The field 'clsMachine._fingerprintversion' is never used

        private int _port;
#pragma warning disable CS0649 // Field 'clsMachine._machineno' is never assigned to, and will always have its default value 0
        private int _machineno,_LastErrCode,_AttdLogCount;
#pragma warning restore CS0649 // Field 'clsMachine._machineno' is never assigned to, and will always have its default value 0
        private string _ioflg;

#pragma warning disable CS0169 // The field 'clsMachine._messflg' is never used
#pragma warning disable CS0414 // The field 'clsMachine._finger' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'clsMachine._rfid' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'clsMachine._face' is assigned but its value is never used
#pragma warning disable CS0414 // The field 'clsMachine._autoclear' is assigned but its value is never used
#pragma warning disable CS0169 // The field 'clsMachine._gateinout' is never used
#pragma warning disable CS0169 // The field 'clsMachine._lunchinout' is never used
        private bool _messflg,_autoclear,_lunchinout,_gateinout,_istft, _rfid,_face,_finger;
#pragma warning restore CS0169 // The field 'clsMachine._lunchinout' is never used
#pragma warning restore CS0169 // The field 'clsMachine._gateinout' is never used
#pragma warning restore CS0414 // The field 'clsMachine._autoclear' is assigned but its value is never used
#pragma warning restore CS0414 // The field 'clsMachine._face' is assigned but its value is never used
#pragma warning restore CS0414 // The field 'clsMachine._rfid' is assigned but its value is never used
#pragma warning restore CS0414 // The field 'clsMachine._finger' is assigned but its value is never used
#pragma warning restore CS0169 // The field 'clsMachine._messflg' is never used

        /* Error Codes
         
         1 SUCCESSED
         4 ERR_INVALID_PARAM
         0 ERR_NO_DATA
        -1 ERROR_NOT_INIT
        -2 ERROR_IO
        -3 ERROR_SIZE
        -4 ERROR_NO_SPACE
        -100 ERROR_UNSUPPORT
        
         */


        public clsMachine(string IPAddress,string ioflg)
        {
            _ip = IPAddress;
            _ioflg = ioflg;
            _connected = false;
            _port = 4370;
            _LastErrCode = 0;
            _istft = false;
            _version = "";
            _rfid = false;
            _face = false;
            _finger = false;
            _autoclear = false;
            CZKEM1 = new zkemkeeper.CZKEM();
        }

        public int GetLastErr { 
            get {
                this.CZKEM1.GetLastError(_LastErrCode);
                return _LastErrCode; 
            } 
        }
        
        public void Connect(out string err)
        {
            err = string.Empty;
           
            _LastErrCode = 0;

            if(string.IsNullOrEmpty(_ip))
            {
                err = "IP Address is required..";
                return;
            }

            if (_ioflg == string.Empty)
            {
                err = "I/O Flg need to set before connect..";
                return;
            }

           

            if (!"I|O|B".Contains(_ioflg))
            {
                err = "Invalid I/O Flg required(I,O,B)";
                return;
            }

            //check ping if Success/networkstatus first
            string status = this.PingMachine(out err);
            if(status.ToUpper() != "SUCCESS" )
            {
                err = "Ping Time Out Expired..";
                return;
            }

           
            try
            {
                _connected = this.CZKEM1.Connect_Net(_ip, _port);
                
            }
            catch (Exception ex)
            {
                err = ex.ToString();
                return;
            }

            if (!_connected)
            {
                this.CZKEM1.GetLastError(_LastErrCode);
                err = "Can not connect machine ErrorCode :" + _LastErrCode.ToString();
                return;
            }

            _rfid = true;
            _finger = false;
            _face = false;
            _tableName = "ccmRFIDTransaction";
            _istft = this.CZKEM1.IsTFTMachine(_machineno);
            
        }

        /// <summary>
        /// Get Data File from device
        /// DataFlg Type of the data file to be obtained 
        /// 1. Attendance record data file 
        /// 2. Fingerprint template data file 
        /// 3. None 
        /// 4. Operation record data file 
        /// 5. User information data file 
        /// 6. SMS data file 
        /// 7. SMS and user relationship data file 
        /// 8. Extended user information data file 
        /// 9. Work code data file
        /// FileName Name of the obtained data file 
        /// </summary>
        /// <param name="DataFlag"></param>
        /// <param name="FileName">File Name with full Path of user's PC</param>

        ///
        public bool GetDataFile(int DataFlag,string FileName,out string err)
        {
            err = string.Empty;
            bool ret = false;
            if(!this._connected){
                err = "Machine is not connected";
                return ret;
            }
            
            ret = this.CZKEM1.GetDataFile(this.CZKEM1.MachineNumber, DataFlag, FileName);
            return ret;

        }

        public bool ReadDataFile(int DataFlag, string FileName, string FilePath, out string err)
        {
            err = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }
            //pending
            ret = this.CZKEM1.ReadFile(this.CZKEM1.MachineNumber,FileName,FilePath);
            return ret;
        }

        public bool GetSDKVersion(out string version, out string err)
        {
            err = string.Empty;
            version = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            ret = this.CZKEM1.GetSDKVersion(ref version);
            return ret;
        }

        public bool GetSerialNumber(out string strSerialNo, out string err)
        {
            err = string.Empty;
            strSerialNo = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            ret = this.CZKEM1.GetSerialNumber(this.CZKEM1.MachineNumber, out strSerialNo);
            return ret;
        }

        public bool GetFirmwareVersion(out string strFirmwarever, out string err)
        {
            err = string.Empty;
            strFirmwarever = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            ret = this.CZKEM1.GetFirmwareVersion(this.CZKEM1.MachineNumber, ref strFirmwarever);
            return ret;
        }

        public bool GetDeviceMAC(out string strmacadd, out string err)
        {
            err = string.Empty;
            strmacadd = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            ret = this.CZKEM1.GetDeviceMAC(this.CZKEM1.MachineNumber, ref strmacadd);
            return ret;
        }

        public bool GetPlatform(out string strplatform, out string err)
        {
            err = string.Empty;
            strplatform = string.Empty;
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            ret = this.CZKEM1.GetPlatform(this.CZKEM1.MachineNumber, ref strplatform);
            return ret;
        }

        /** Not Required
        public bool SaveDeviceData(out string err)
        {
            
            bool ret = false;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }
            
            
            string sdkversion = "";
            string serialno = "";
            string firmwarever = "";
            string platform = "";
            string macadd = "";
            
            int UserCapacity = 0;
            int RegisteredUsers = 0;
            int FaceCapacity = 0;
            int RegisteredFace = 0;
            int FingerCapacity = 0;
            int RegisteredFinger = 0;
            
            this.GetSDKVersion(out sdkversion, out err);
            this.GetSerialNumber(out serialno, out err);
            this.GetFirmwareVersion(out firmwarever, out err);
            this.GetPlatform(out platform, out err);
            this.GetDeviceMAC(out macadd, out err);
            this.Get_StatusInfo_Users(out RegisteredUsers, out UserCapacity, out err);
            this.Get_StatusInfo_Face(out RegisteredFace, out FaceCapacity, out err);
            this.Get_StatusInfo_Finger(out RegisteredFinger, out FingerCapacity, out err);

            using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        string sql = "Update ReaderConfig set FirmwareVer ='" + firmwarever + "'," +
                            " SdkVer ='" + sdkversion + "', SerialNo ='" + serialno + "',Platform ='" + platform + "'," +
                            " MacAdd ='" + macadd + "'," +
                            " UserCapacity = '" + UserCapacity.ToString() + "'," +
                            " RegisteredUsers ='" + RegisteredUsers.ToString() + "'," +
                            " FaceCapacity='" + FaceCapacity.ToString() + "'," +
                            " RegisteredFace ='" + RegisteredFace.ToString() + "'," +
                            " FingerCapacity='" + FingerCapacity.ToString() + "'," +
                            " RegisteredFinger='" + RegisteredFinger.ToString() + "'," +
                            " DeviceInfoUpdDt ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                            " Where MachineIP ='" + this._ip + "'";

                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        err = "";
                        return true;

                    }
                    catch (Exception ex)
                    {
                        err = ex.Message.ToString();
                        return false;
                    }
                }
            }

        }


        public int GetAccessRecords(out string err)
        {
            err = string.Empty;
            int ret = 0;
            if (!this._connected)
            {
                err = "Machine is not connected";
                return ret;
            }

            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;           
            int odwEnrollNumber = 0; //for old machine            
            int Params4 = 0;
            int Params3 = 0;
            int Params2 = 0;
            int Params1 = 0;
            int dwManipulation = 0;
            bool m_tft = false;

            bool logfound = false;

            m_tft = this.CZKEM1.IsTFTMachine(_machineno);

            this.CZKEM1.EnableDevice(_machineno, false);//disable the device
            if (this.CZKEM1.ReadAllSLogData(_machineno))//read all the operation records to the memory
            {
                
                string sql = "SELECT isnull(Max(ReqNo),0) + 1 from MastMachineAccessLog ";
                int ReqNo = Convert.ToInt32(Utils.Helper.GetDescription(sql, Utils.Helper.constr,out err));
                
                if (!string.IsNullOrEmpty(err))
                {
                    ret = 0;
                    return ret;
                }

                //string tpath = Utils.Helper.GetInfoLogFilePath();
                //string filename = System.IO.Path.Combine(tpath, "5_data.dat");
                //this.GetDataFile(4, filename, out err);

                err = string.Empty;
                DateTime ReqDt = DateTime.Now;
                string ReqBy = Utils.User.GUserID;
                string Machid = this._ip;

                
                int outnum = 0;
                string stradmin = string.Empty;
                string struser = string.Empty;
                string strtime = string.Empty;


                while (CZKEM1.SSR_GetSuperLogData(_machineno, out outnum, out stradmin, out struser, out dwManipulation, out strtime, out Params1, out Params2, out Params3))

                //while (CZKEM1.GetSuperLogData(_machineno, ref _machineno, ref odwEnrollNumber,
                //        ref Params4,ref Params1,ref Params2, ref dwManipulation,ref Params3,
                //        ref idwYear,ref idwMonth,ref idwDay,ref idwHour,ref idwMinute))//get records from the memory
                {
                    DateTime logdt = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute,0);

                    using (SqlConnection cn = new SqlConnection(Utils.Helper.constr))
                    {
                        try
                        {
                            cn.Open();
                        }
                        catch (Exception ex)
                        {
                            ret = 0;
                            err = ex.Message.ToString();
                            return ret;
                        }

                        sql = "Insert into MastMachineAccessLog (ReqNo,MachineIP,ReqDt,ReqBy,ManiplulationTime," +
                            " DwManipulation,Params1,Params2,Params3,Params4,OperatedUserID) Values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";

                        sql = string.Format(sql,
                            ReqNo, Machid, ReqDt.ToString("yyyy-MM-dd HH:mm:ss"), ReqBy,
                            logdt.ToString("yyyy-MM-dd HH:mm:ss"),
                            dwManipulation, Params1, Params2, Params3, Params4, odwEnrollNumber
                            );

                        //sql = "Insert into MastMachineAccessLog (ReqNo,MachineIP,ReqDt,ReqBy,StringTime," +
                        //    " DwManipulation,Params1,Params2,Params3,Params4,StringAdmin,StringUser,OutNumber) Values ({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')";

                        //sql = string.Format(sql,
                        //    ReqNo, Machid, ReqDt.ToString("yyyy-MM-dd HH:mm:ss"), ReqBy,
                        //    strtime,
                        //    dwManipulation, Params1, Params2, Params3, Params4, stradmin, struser, outnum
                        //    );

                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sql;

                            try
                            {
                                cmd.ExecuteNonQuery();
                                logfound = true;
                            }
                            catch (Exception ex)
                            {
                                err = ex.Message.ToString();
                                ret = 0;
                            }
                                                   
                        }

                        ret = ReqNo;
                    }
                    
                }
                
            }
            if (logfound)
                this.CZKEM1.ClearSLog(_machineno);
            
            this.CZKEM1.EnableDevice(_machineno, true);//disable the device
            return ret;
        }
        **/
       
        public void DisConnect (out string err)
        {
            err = string.Empty;
            

            if(string.IsNullOrEmpty(_ip))
            {
                err = "IP Address is required..";
                return;
            }
            try
            {
                this.CZKEM1.Disconnect();
                _connected = false;
                return;
            }catch(Exception ex ){

                this.CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString() + Environment.NewLine + ex.ToString();
                _connected = true;
            }
            
        }

        public void GetAttdCnt(out int count, out string err)
        {
            count = 0;
            err = string.Empty;

            if(!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            CZKEM1.EnableDevice(_machineno, false);//disable the device
            if (CZKEM1.GetDeviceStatus(_machineno, 6, ref _AttdLogCount)) //Here we use the function "GetDeviceStatus" to get the record's count.The parameter "Status" is 6.
            {
                count = _AttdLogCount;
            }
            else
            {
               CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString();
            }
            CZKEM1.EnableDevice(_machineno, true);//enable the device
        }

        public void GetAttdRec(out List<AttdLog> AttdLogRec,out string err)
        {
            AttdLogRec = new List<AttdLog>();


            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

           


            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;
            string sdwEnrollNumber = "";
            int odwEnrollNumber = 0; //for old machine
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            
            int idwReserved = 0;
           
            bool m_tft = false;

            //count records
            int cnt = 0; string outerr = string.Empty;
            this.GetAttdCnt(out cnt, out outerr);

            if (cnt == 0)
            {
                err = "No Records..";
                return;
            }
            
            m_tft = CZKEM1.IsTFTMachine(_machineno);

            _istft = m_tft;

            //'Prepare File Name for writing log data
            //CZKEM1.GetDeviceTime(_machineno, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond);
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "NewTech","PunchData");

            string filenm = "RFID_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "-[" + _ip.Replace(".","_") + "].txt";
            string fullpath = Path.Combine(filepath, filenm);
            string write_err = string.Empty;

            DirectoryInfo di = Directory.CreateDirectory(filepath);

            CZKEM1.EnableDevice(_machineno, false);//disable the device
            if (CZKEM1.ReadGeneralLogData(_machineno))//read all the attendance records to the memory
            {               
                
                
                if (m_tft)
                {
                    while (CZKEM1.SSR_GetGeneralLogData(_machineno, out sdwEnrollNumber, out idwVerifyMode,
                            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {

                        AttdLog t = new AttdLog();
                        DateTime logdt = new DateTime(idwYear,idwMonth,idwDay,idwHour,idwMinute,idwSecond);
                        

                        t.EmpUnqID = sdwEnrollNumber.ToString();
                        t.PunchDate = logdt;
                        t.MachineIP = _ip;
                        t.IOFLG = _ioflg;
                        
                        t.AddID = "system";
                        t.AddDt = DateTime.Now;
                       
                        t.TableName = _tableName;
                        AttdLogRec.Add(t);
                        
                    }
                }
                else
                {

                    odwEnrollNumber = 0;

                    while (CZKEM1.GetGeneralExtLogData(_machineno,ref odwEnrollNumber,ref idwVerifyMode,
                            ref idwInOutMode,ref idwYear,ref idwMonth,ref idwDay,ref idwHour,ref idwMinute,ref idwSecond,ref idwWorkcode,ref idwReserved ))//get records from the memory
                    {
                        AttdLog t = new AttdLog();
                        DateTime logdt = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);


                        t.EmpUnqID = odwEnrollNumber.ToString();
                        t.PunchDate = logdt;
                        t.MachineIP = _ip;
                        t.IOFLG = _ioflg;
                       
                        t.AddID = "system";
                        t.AddDt = DateTime.Now;
                        t.TableName = _tableName;
                        AttdLogRec.Add(t);                        
                    }
                }
                
                
            }
            else
            {
                
                this.CZKEM1.GetLastError(ref _LastErrCode);

                if (_LastErrCode != 0)
                {
                    err =  "Reading data from terminal failed,ErrorCode: " +_LastErrCode.ToString();
                    write_err = "Reading data from terminal failed,ErrorCode: " + _LastErrCode.ToString();
                }
                else
                {
                    err = "No Records Found...";
                    write_err = "No Records Found...";
                }
            }
            
            this.CZKEM1.EnableDevice(_machineno, true);//enable the device
            
            if(!string.IsNullOrEmpty(write_err))
            {
                this.CZKEM1.RestartDevice(_machineno);
                return;
            }

            foreach (AttdLog t in AttdLogRec)
            {
                string dberr = AttdLogStoreToDb(t);
                if (!string.IsNullOrEmpty(dberr))
                {
                    t.Error = dberr;
                    write_err += dberr;
                    err += "Error while store to db : " + t.EmpUnqID + " : " + dberr + Environment.NewLine;
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullpath, true))
            {
                file.WriteLine("");
                //write text file and also store in db
                foreach (AttdLog t in AttdLogRec)
                {
                    file.WriteLine(t.ToString());
                }
            }

            string terr = string.Empty;
            //AttdLogClear(out terr);
            
            
        }


        
        /// <summary>
        /// store attendance logs in db
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public string AttdLogStoreToDb(AttdLog t)
        {
            string err = string.Empty;

            using (SqlConnection cn = new SqlConnection(Utility.SQLCnStr))
            {
                try
                {
                    cn.Open();
                    string sql = t.GetDBWriteString();

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("System.Data.SqlClient.SqlException (0x80131904): Violation of PRIMARY KEY"))
                    {

                    }
                    else
                    {
                        err = ex.ToString();
                    }

                    //try
                    //{
                    //    string sql = t.GetDBWriteErrString();

                    //    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    //    {
                    //        cmd.ExecuteNonQuery();
                    //        //err = "Duplicate Data Found..";
                    //    }

                    //}catch(Exception ex1)
                    //{
                    //    err += ex1.ToString();
                    //}
                    
                }
            }

            return err;
        }

        /// <summary>
        /// clear the attendance records
        /// </summary>
        /// <param name="err"></param>
        public void AttdLogClear(out string err)
        {
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            this.CZKEM1.EnableDevice(_machineno, false);//disable the device
            
            
            if (CZKEM1.ClearGLog(_machineno))
            {
                CZKEM1.RefreshData(_machineno);//the data in the device should be refreshed
            }
            else
            {
                CZKEM1.GetLastError(ref _LastErrCode);
                err = "Operation failed,ErrorCode=" + _LastErrCode.ToString();
            }

            CZKEM1.EnableDevice(_machineno, true);//enable the device
        }

        /// <summary>
        /// set the system's current time in machine
        /// </summary>
        /// <param name="err"></param>
        public void SetTime(out string err)
        {
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }
            
            
            this.CZKEM1.EnableDevice(_machineno,false);
            


            err = (this.CZKEM1.SetDeviceTime(_machineno) ? "" : "Unable to Set Time...");

            this.CZKEM1.EnableDevice(_machineno, true);
        }

        /// <summary>
        /// restart machine
        /// </summary>
        /// <param name="err"></param>
        public void Restart(out string err)
        {
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            bool t = CZKEM1.RestartDevice(_machineno);
            
        }

        
        public bool SetDuplicatePunchDuration(int nosofminutes)
        {
            bool result = false;
            //if (!_messflg)
            //{
                ////// get duplicate punch time from machine
                //int duptime = 0;
                //result = this.CZKEM1.GetDeviceInfo(_machineno, 8, ref duptime);

                //if (result)
                //{
                    result = this.CZKEM1.SetDeviceInfo(_machineno, 8, nosofminutes);
                //}
            //}            
            return result;
        }

        public void Register(string tEmpUnqID, string RFIDNO , out string err)
        {
            
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }
            if (string.IsNullOrEmpty(tEmpUnqID))
            {
                err = "UserID is required..";
                return;
            }

            

            UserBioInfo emp = new UserBioInfo();
            emp.UserID = tEmpUnqID;
            emp.CardNumber = RFIDNO;

            

            if (string.IsNullOrEmpty(emp.CardNumber))
            {
                err = "RFID Card Number not found...";
                return;
            }
            else
            {
                int t = 0;
                int.TryParse(emp.CardNumber, out t);
                if (t <= 0)
                {
                    err = "RFID Card Number is Required...";
                    return;
                }
            }

            if(!_istft)
            {
                this.CZKEM1.set_CardNumber(0,Convert.ToInt32(emp.CardNumber));
                bool x = this.CZKEM1.SetUserInfo(_machineno, Convert.ToInt32(emp.UserID), "", "", 0,true);
                if (x)
                {
                    err = "Registerd to " + _ip;
                    string err2 = string.Empty;
                    this.StoreHistory(emp.UserID, "Register", out err2);
                }                    
                else
                {
                    this.CZKEM1.GetLastError(_LastErrCode);
                    err = "Error : No " + _LastErrCode.ToString() + " while registrator in " + _ip;
                }

                return;
            }
            
            if(this.CZKEM1.SetStrCardNumber(emp.CardNumber))
            {
                this.StoreHistory(emp.UserID, "Register", out err);
                err = "Registerd to " + _ip;
                if (this.CZKEM1.SSR_SetUserInfo(_machineno, emp.UserID, "", "", 0, true))
                {                    
                    this.CZKEM1.SetUserGroup(_machineno, Convert.ToInt32(emp.UserID), 1);
                    
                    return;
                }
                else
                {
                    this.CZKEM1.GetLastError(_LastErrCode);
                    err = "Error : No " + _LastErrCode.ToString() + " while setting user group " + _ip;
                }

            }
            else
            {
                this.CZKEM1.GetLastError(_LastErrCode);
                err = "Error : No " + _LastErrCode.ToString() + " while registrator in " + _ip;
            }

        }


        public void EnableDevice(bool isEnabled)
        {
            this.CZKEM1.EnableDevice(_machineno, isEnabled);
        }



        public void RefreshData()
        {
            this.CZKEM1.RefreshData(_machineno);
        }
               
        public void DeleteUser(string tEmpUnqID, out string err)
        {
            err = string.Empty;
            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }
            if (string.IsNullOrEmpty(tEmpUnqID))
            {
                err = "UserID is required..";
                return;
            }           

            this.CZKEM1.EnableDevice(_machineno, false);
            
            if (!_istft)
            {
               
                bool t = this.CZKEM1.DeleteEnrollData(_machineno, Convert.ToInt32(tEmpUnqID), _machineno, 0);
                if (t)
                {
                    err = "Deleted from " + this._ip;
                    string err2 = string.Empty;
                    this.StoreHistory(tEmpUnqID, "Delete", out err2);
                }
                else
                {
                    this.CZKEM1.GetLastError(_LastErrCode);
                    err = "Error : No " + _LastErrCode.ToString() + " while deleting from " + _ip;

                }

                        
            }
            else
            {
                    
                bool t = this.CZKEM1.SSR_DeleteEnrollDataExt(_machineno, tEmpUnqID, 12);
                if (t)
                {
                    err = "Deleted from " + this._ip;
                    string err2 = string.Empty;
                    this.StoreHistory(tEmpUnqID, "Delete", out err2);
                }                    
                else
                {
                    this.CZKEM1.GetLastError(_LastErrCode);
                    err = "Error : No " + _LastErrCode.ToString() + " while deleting from " + _ip;
                }
            }
                             
            this.CZKEM1.RefreshData(_machineno);
            //this.CZKEM1.EnableDevice(_machineno, true); 
        }

       
        public string PingMachine(out string err)
        {
            string status = string.Empty;
            err = string.Empty;

            if(string.IsNullOrEmpty(_ip))
            {
                err = "IP Address is required..";
                status = "Bad Request";
            
                return status;
            }

            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(_ip, 15000);

                if (reply.Status == IPStatus.Success)
                {
                    status = "Success";
                }
                else
                {
                    status = reply.Status.ToString();
                }
                
            }
            catch(Exception ex)
            {
                status = "Request timeout";
                err = ex.ToString();
            }

            return status;
        }

        public void DeleteAllUser(out string err)
        {
            err = string.Empty;


            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            bool vRet = this.CZKEM1.ReadAllUserID(_machineno); // 'read all the user information to the memory
            if (!vRet)
            {
                err = "Error : Can not read All UserID";
                return;
            }

            string _userid, _username, _password, _cardno;
            int _prev, _useridInt;
            bool _enabled = false;

            this.CZKEM1.EnableDevice(_machineno, false);

            _userid = string.Empty; _username = string.Empty; _password = string.Empty; _cardno = string.Empty;
            _useridInt = 0; _prev = 0; _enabled = false;

            List<UserBioInfo> tUserList = new List<UserBioInfo>();

            if (_istft)
            {
                while (this.CZKEM1.SSR_GetAllUserInfo(_machineno, out _userid, out _username, out _password, out _prev, out _enabled))
                {
                    UserBioInfo t = new UserBioInfo();
                    t.UserID = _userid;
                    t.Previlege = _prev;
                    t.Enabled = _enabled;
                    tUserList.Add(t);

                }//end while

                foreach (UserBioInfo t in tUserList)
                {
                    this.CZKEM1.SSR_DeleteEnrollDataExt(_machineno, t.UserID, 12);
                    //this.CZKEM1.DelUserFace(_machineno, t.UserID, 50);   
                }

            }//end if new machine
            else
            {
                //old machines
                while (this.CZKEM1.GetAllUserInfo(_machineno, ref _useridInt, ref _username, ref _password, ref _prev, ref _enabled))
                {
                    UserBioInfo t = new UserBioInfo();
                    t.UserID = _useridInt.ToString();
                    t.Previlege = _prev;
                    t.Enabled = _enabled;
                    tUserList.Add(t);
                }

                foreach (UserBioInfo t in tUserList)
                {
                    this.CZKEM1.DeleteEnrollData(_machineno, Convert.ToInt32(t.UserID), _machineno, 0);
                }
            }
            this.CZKEM1.EnableDevice(_machineno, false);
            this.RefreshData();


        }

        public void ClearALLUserData(out string err)
        {
            err = string.Empty;


            if (!_connected)
            {
                err = "Machine not connected..";
                return;
            }

            bool vRet = this.CZKEM1.ClearData(_machineno,5); // 'read all the user information to the memory
            
            if(_istft)
                vRet = this.CZKEM1.ClearData(_machineno, 2);

            if (!vRet)
            {
                err = "Error : Can not Delete All Users....";
                return;
            }

        }

        public bool SetUserGroup(int Userid,int usergroup, out string err)
        {
            err = string.Empty;
            bool res = false;

            if (!_connected)
            {
                err = "Machine not connected..";
                return res;
            }
            
            
            //res = this.CZKEM1.SetUserInfoEx(1,Userid,0,new byte());
            res = this.CZKEM1.SetUserGroup (1,Userid,usergroup);

            return res;
        }

        public void StoreHistory(string Empcode,string operation,out string err)
        {
            err = "";
            using (SqlConnection cn = new SqlConnection(Utility.SQLCnStr))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    return;
                }

                
                string  sql = "Insert into ccmRFIDHistory (MachineIP,OperatorCode,Operation,AddDt) values " +
                        " ('" + this._ip + "','" + Empcode + "'," +
                        " '" + operation + "',GetDate() ) ";


                int rescnt = 0;
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = cn;
                        rescnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { err = ex.Message; }

                    
                }//using command

            }//using connection
        }
    }

    public class UserBioInfo
    {
        public string UserID, UserName, WrkGrp, Password, CardNumber, FaceTemp, FingerTemp, MessCode, MessGrpCode, err;

        public long Previlege;
        public bool Enabled;
        public long VerifyStyle;
        public int FaceLength = 0;
        public int FaceIndex = 50;
        public int FingerIndex = 0;
        public int FingerLength = 0;

        public UserBioInfo()
        {
            UserID = "";
            UserName = "";
            WrkGrp = "";
            Password = "";
            Previlege = 0;
            Enabled = false;
            VerifyStyle = 0;
            FaceTemp = "";
            FingerTemp = "";
            FaceLength = 0;
            FingerLength = 0;
            MessCode = "";
            MessGrpCode = "";
            err = "";
        }

      

    }

    class AttdLog
    {
        public string EmpUnqID;
        public DateTime PunchDate;
        public string IOFLG;
        public string MachineIP;
        
        public DateTime AddDt;
        public string AddID;
        public string TableName;
        public string Error;
        
        public override string ToString()
        {
            return IOFLG.ToString() + ";" + PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + ";" + EmpUnqID.ToString() + ";" + MachineIP ;
        }

        public string GetDBWriteString()
        {
            string dbstr = string.Empty;
            if (this.TableName == string.Empty)
                this.TableName = "ccmRFIDTransaction";

            dbstr = "Insert into " + this.TableName.Trim() + " (PunchTime,OperatorCode,MachineIP,AddDt) Values (" +
                " '" + this.PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.EmpUnqID + "','" + this.MachineIP + "'," +
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "');";

            if (this.PunchDate.Year == 2000)
            {
                string errdbstr = GetDBWriteErrString();
                return errdbstr;

            }



            return dbstr;
        }

        public string GetDBWriteErrString()
        {
            string dbstr = string.Empty;

            //if duplicate punch found place in ccmRFIDTransactionErr
            dbstr = "Insert into ccmRFIDTransactionErr (PunchTime,OperatorCode,MachineIP,AddDt) Values (" +
                " '" + this.PunchDate.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.EmpUnqID + "','" + this.MachineIP + "'," +
                " '" + this.AddDt.ToString("yyyy-MM-dd HH:mm:ss") + "');";

            return dbstr;
        }
    }
}
