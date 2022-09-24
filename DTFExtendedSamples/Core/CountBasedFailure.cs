namespace DTFExtendedSamples.Core
{
    public class CountBasedFailure: IRandomFailure
    {
        private int _count;
        
        public void EventuallyFails()
        {
            lock (this)
            {
                if (++_count == 3)
                {
                    _count = 0;
                    throw new RandomFailureException();
                }
            }
        }
    }
}