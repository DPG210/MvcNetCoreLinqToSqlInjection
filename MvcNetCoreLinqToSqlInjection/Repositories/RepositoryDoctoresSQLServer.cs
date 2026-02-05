using Microsoft.Data.SqlClient;
using MvcNetCoreLinqToSqlInjection.Models;
using System.Data;

namespace MvcNetCoreLinqToSqlInjection.Repositories
{
    public class RepositoryDoctoresSQLServer
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
    }
}
