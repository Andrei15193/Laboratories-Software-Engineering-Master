using System;
using Tourist.Models;
namespace Tourist.Data
{
    public class AttractionsToVisitEventArgs
        : EventArgs
    {
        public AttractionsToVisitEventArgs(Attraction attraction)
        {
            if (attraction == null)
                throw new ArgumentNullException("attraction");

            Attraction = attraction;
        }

        public Attraction Attraction
        {
            get;
            private set;
        }
    }
}