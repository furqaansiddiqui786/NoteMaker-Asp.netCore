using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteMaker.Models.ViewModels
{
    public class NotesViewModel
    {
        public Notes Notes { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
