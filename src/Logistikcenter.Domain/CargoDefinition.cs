namespace Logistikcenter.Domain
{
    public class CargoDefinition
    {
        private double volume;

        public virtual double Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public CargoDefinition()
        {
        }

        public CargoDefinition(double volume)
        {
            this.volume = volume;
        }
    }
}
