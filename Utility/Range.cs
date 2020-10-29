namespace Game1.Utility
{
    //範囲
    class Range
    {
        public int first { set; get; }//範囲の最初
        public int last { set; get; }//範囲の最後

        public Range(int first, int last)
        {
            this.first = first;
            this.last = last;
        }

        //範囲内か
        public bool IsRange(int num)
        {
            if (num < first || num > last)
            {
                return false;
            }

            return true;
        }
    }
}
