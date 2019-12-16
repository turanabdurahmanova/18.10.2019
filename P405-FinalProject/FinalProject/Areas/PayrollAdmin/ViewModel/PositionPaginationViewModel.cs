using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class PositionPaginationViewModel
    {
        public PaginationModel PaginationModel { get; set; }
        public IEnumerable<Position> Positions { get; set; }
        public Position Position { get; set; }
        public SelectList Departaments { get; set; }

    }
}
