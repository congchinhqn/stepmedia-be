﻿﻿using System;

  namespace Metatrade.Core.Extensions
{
    public static class Extensions
    {
        public static string ToFriendlyErrorMessage(this Exception ex)
        {
            return
                $"{ex.Message} - {((ex.InnerException != null) ? ex.InnerException.Message : string.Empty)} - {ex.StackTrace}";
        }
    }
}
