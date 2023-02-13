var CheckIfMobile = {
   IsMobile: function()
   {
      return UnityLoader.SystemInfo.mobile;
   }
};

mergeInto(LibraryManager.library, CheckIfMobile);