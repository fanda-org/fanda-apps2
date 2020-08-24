using System;

namespace Fanda.Service.Dto
{
    public class UnitConversionDto
    {
        public Guid FromUnitId { get; set; }
        public Guid ToUnitId { get; set; }
        public byte CalcStep { get; set; }
        public char Operator { get; set; }
        public decimal Factor { get; set; }
        public bool Active { get; set; }
    }
}
