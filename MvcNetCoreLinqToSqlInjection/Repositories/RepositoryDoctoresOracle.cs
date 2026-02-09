using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcNetCoreLinqToSqlInjection;
using MvcNetCoreLinqToSqlInjection.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static Azure.Core.HttpHeader;

#region PROCEDIMIENTOS_ALMACENADOS
//create or replace procedure SP_DELETE_DOCTOR
//(p_iddoctor DOCTOR.DOCTOR_NO%type)
//AS
//    BEGIN
//        delete from DOCTOR where DOCTOR_NO= p_iddoctor;
//commit;
//END;
//create or replace procedure SP_UPDATE_DOCTOR
//(p_iddoctor DOCTOR.DOCTOR_NO%type, p_idhospital DOCTOR.HOSPITAL_COD%type, p_apellido DOCTOR.APELLIDO%type, p_especialidad DOCTOR.ESPECIALIDAD%type, p_salario DOCTOR.SALARIO%type)
//as
//begin
// update DOCTOR set hospital_cod=p_idhospital, APELLIDO = p_apellido, ESPECIALIDAD = p_especialidad, SALARIO = p_salario where doctor_no=p_iddoctor;
//commit;
//end;

#endregion

namespace MvcNetCoreLinqToSqlInjection.Repositories
   
{
    public class RepositoryDoctoresOracle : IRepositoryDoctores
    {
        private DataTable tablaDoctor;
        private OracleConnection cn;
        private OracleCommand com;
        public RepositoryDoctoresOracle()
        {
            string connectionString = "Data Source=LOCALHOST:1521/FREEPDB1; Persist Security Info=true;User Id=SYSTEM;Password=oracle ";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from doctor";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaDoctor = new DataTable();
            ad.Fill(this.tablaDoctor);
        }

        public List<Doctor> GetDoctores()
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable() select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("Doctor_NO"),
                    Apellido = row.Field<string>("Apellido"),
                    Especialidad = row.Field<string>("especialidad"),
                    Salario = row.Field<int>("salario"),
                    IdHospital = row.Field<int>("hospital_cod")
                };
                doctores.Add(doc);
            }
            return doctores;
        }
        public async Task CreateDoctorAsync(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "insert into DOCTOR values(:idHospital,:id,:apellido,:especialidad,:salario)";
            OracleParameter pamIdHospital = new OracleParameter(":idHospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":id", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamEspecialidad = new OracleParameter(":especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            this.com.Parameters.Add(pamIdHospital);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspecialidad);
            this.com.Parameters.Add(pamSalario);
            

            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task DeleteDoctorAsync(int idDoctor)
        {
            string sql = "SP_DELETE_DOCTOR";
            OracleParameter pamId = new OracleParameter(":p_iddoctor", idDoctor);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateDoctorAsync(int idDoctor, string apellido, string especialidad, int salario, int idHospital)
        {
            string sql = "SP_UPDATE_DOCTOR";
            OracleParameter pamIdHospital = new OracleParameter(":p_idhospital", idHospital);
            OracleParameter pamIdDoctor = new OracleParameter(":p_iddoctor", idDoctor);
            OracleParameter pamApellido = new OracleParameter(":p_apellido", apellido);
            OracleParameter pamEspecialidad = new OracleParameter(":p_especialidad", especialidad);
            OracleParameter pamSalario = new OracleParameter(":p_salario", salario);
            this.com.Parameters.Add(pamIdDoctor);
            this.com.Parameters.Add(pamIdHospital);
            
            this.com.Parameters.Add(pamApellido);
            this.com.Parameters.Add(pamEspecialidad);
            this.com.Parameters.Add(pamSalario);


            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task<Doctor> FindDoctor(int idDoctor)
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable() 
                           where datos.Field<int>("doctor_no")==idDoctor
                           select datos;
            Doctor doctor = new Doctor();
            var row = consulta.First();

            doctor.IdDoctor = row.Field<int>("Doctor_NO");
            doctor.Apellido = row.Field<string>("Apellido");
            doctor.Especialidad = row.Field<string>("especialidad");
            doctor.Salario = row.Field<int>("salario");
            doctor.IdHospital = row.Field<int>("hospital_cod");
              
            return doctor;
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            var consulta = from datos in this.tablaDoctor.AsEnumerable()
                           where (datos.Field<string>("especialidad")).ToUpper().StartsWith(especialidad.ToUpper())
                           select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach (var row in consulta)
            {
                Doctor doc = new Doctor
                {
                    IdDoctor = row.Field<int>("Doctor_NO"),
                    Apellido = row.Field<string>("Apellido"),
                    Especialidad = row.Field<string>("especialidad"),
                    Salario = row.Field<int>("salario"),
                    IdHospital = row.Field<int>("hospital_cod")
                };
                doctores.Add(doc);
            }
            return doctores;
        }

    }
}
