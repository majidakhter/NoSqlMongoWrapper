﻿using System.Collections.Generic;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Helpers
{
    public class StoreResult<T>
    {
        public StoreResult(bool success, T document, IEnumerable<T> documents)
        {
            Success = success;
            Document = document;
            Documents = documents;       
        }
        public T Document { get; set; }  
        public IEnumerable<T> Documents { get; set; }
        public bool Success { get; set; }       
    }
}
