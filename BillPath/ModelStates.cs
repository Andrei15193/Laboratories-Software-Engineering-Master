namespace BillPath
{
    public static class ModelStates
    {
        private static volatile ModelStateCache _globalCache = new ModelStateCache();

        public static ModelState GetFor(object model)
            => _globalCache.GetFor(model);

        public static void Clear()
            => _globalCache = new ModelStateCache();
    }
}