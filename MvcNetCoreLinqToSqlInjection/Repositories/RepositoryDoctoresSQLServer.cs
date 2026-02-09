using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcNetCoreLinqToSqlInjection.Models;
using System.Data;

#region PROCEDIMIENTOS_ALMACENADOS
//create procedure SP_DELETE_DOCTOR
//(@iddoctor int)
//as
//	delete from DOCTOR where DOCTOR_no=@iddoctor
//go
//create procedure SP_UPDATE_DOCTOR
//(@iddoctor int, @idhospital int, @apellido nvarchar(50), @especialidad nvarchar(50), @salario int)
//as
// update DOCTOR set hospital_cod=@idhospital, APELLIDO = @apellido, ESPECIALIDAD = @especialidad, SALARIO = @salario where doctor_no=@iddoctor
//go
#endregion

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresSQLServer : IRepositoryDoctores
    {
        SqlConnection cn;
        SqlCommand com;
        DataTable tablaDoctor;
        public RepositoryDoctoresSQLServer()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from doctor";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaDoctor = new DataTable();
            ad.Fill(this.tablaDoctor);
        }

        public List<Doctor> GetDoctores()
        {
            var consulta =  from datos in this.tablaDoctor.AsEnumerable() select datos;
            List<Doctor> doctores = new List<Doctor>();
            foreach(var row in consulta)
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
            string sql = "insert into doctor values(@idHospital,@id,@apellido,@especialidad,@salario)";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.Parameters.AddWithValue("@id", idDoctor);
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
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
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
            this.com.Parameters.AddWithValue("@idhospital", idHospital);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.Parameters.AddWithValue("@iddoctor", idDoctor);
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
                           where datos.Field<int>("doctor_no") == idDoctor
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

        

        public  List<Doctor> GetDoctoresEspecialidad(string especialidad)
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
