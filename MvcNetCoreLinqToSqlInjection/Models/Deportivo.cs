namespace MvcNetCoreLinqToSqlInjection.Models
{
    public class Deportivo: ICoche
    {
        public string Marca { get; set ; }
        public string Modelo { get ; set ; }
        public string Imagen { get ; set ; }
        public int Velocidad { get ; set ; }
        public int VelocidadMaxima { get ; set ;}


        public Deportivo()
        {
            this.Marca = "Guay";
            this.Marca = "Deportivo to guapo";
            this.VelocidadMaxima = 300;
            this.Imagen = "AstonMartin.jfif";
        }

        
        public void Acelerar()
        {
            this.Velocidad += 35;
            if (this.Velocidad >= this.VelocidadMaxima)
            {
                this.Velocidad = this.VelocidadMaxima;
            }
          
        }

        public void Frenar()
        {
            this.Velocidad -= 20;
            if (this.Velocidad < 0)
            {
                this.Velocidad = 0;
            }
           
        }
    }
}
