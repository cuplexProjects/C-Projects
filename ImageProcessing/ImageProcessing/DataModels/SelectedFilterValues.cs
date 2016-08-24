namespace ImageProcessing.DataModels
{
    public class SelectedFilterValues
    {
        public double Gamma { get; set; }
        public double Smooth { get; set; }
        public double Sharpen { get; set; }
        public double Gaussian { get; set; }

        public SelectedFilterValues()
        {
            //Set defaults
            Gamma = 1;
            Smooth = 1;
            Sharpen = 1;
            Gaussian = 1;
        }
    }
}