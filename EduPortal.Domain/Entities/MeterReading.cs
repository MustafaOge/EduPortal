namespace EduPortal.Domain.Entities
{
    public class MeterReading : BaseEntity<int>
    {
        public MeterReading(DateTime readingDate)
        {
            ReadingDate = readingDate;
            LastIndexDate = readingDate.AddDays(30);
        }

        public DateTime ReadingDate { get; set; }
        public decimal TotalIndex { get; set; }
        public decimal TotalFirstIndex { get; set; } // Toplam Endeksin İlk Değeri
        public decimal TotalLastIndex { get; set; } // Toplam Endeksin Son Değeri
        public decimal DayFirstIndex { get; set; } // Gündüzün İlk Endeksi
        public decimal DayLastIndex { get; set; } // Gündüzün Son Endeksi
        public decimal PeakFirstIndex { get; set; } // Puantın İlk Endeksi
        public decimal PeakLastIndex { get; set; } // Puantın Son Endeksi
        public decimal NightFirstIndex { get; set; } // Gece nin İlk Endeksi
        public decimal NightLastIndex { get; set; } // Gece nin Son Endeksi
        public decimal TotalDifference { get; set; } // Toplam Fark
        public decimal DayDifference { get; set; } // Gündüz Farkı
        public decimal PeakDifference { get; set; } // Puant Farkı
        public decimal NightDifference { get; set; } // Gece Farkı
        public Invoice Invoice { get; set; }
        public int InvoiceId { get; set; }
        public int ReadingDayDifference { get; set; }
        public DateTime LastIndexDate { get; set; }

    }
}

