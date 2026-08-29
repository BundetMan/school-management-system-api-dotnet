namespace SchoolAPI.DTOs.Reports
{
    public class DashboardFilterDto
    {
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? LevelId { get; set; }   // grade filter
        public string? ClassId { get; set; }   // class filter
    }

    public class DashboardSummaryDto
    {
        public int TotalStudents { get; set; }
        public int TotalRegistrations { get; set; }
        public int PendingRegistrations { get; set; }
        public int ApprovedRegistrations { get; set; }
        public int RejectedRegistrations { get; set; }
        public int WaitlistedStudents { get; set; }

        public decimal TotalFeesCollected { get; set; }
        public List<PaymentStatusCountDto> PaymentStatusBreakdown { get; set; } = [];

        public bool UnpaidBreakdownIsPointInTime { get; set; }

        public List<MonthlyCountDto> MonthlyRegistrations { get; set; } = [];
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = [];

        public List<GradeCountDto> StudentsByGrade { get; set; } = [];
        public List<ClassCountDto> StudentsByClass { get; set; } = [];
        public List<StatusDistributionDto> RegistrationStatusDistribution { get; set; } = [];
    }

    public class MonthlyCountDto { 
        public int Year { get; set; } 
        public int Month { get; set; } 
        public int Count { get; set; } 
    }
    public class MonthlyRevenueDto { 
        public int Year { get; set; } 
        public int Month { get; set; } 
        public decimal Total { get; set; } 
    }
    public class GradeCountDto { 
        public string LevelId { get; set; } = String.Empty;
        public string LevelName { get; set; } = ""; 
        public int StudentCount { get; set; } 
    }
    public class ClassCountDto { 
        public string ClassId { get; set; } = String.Empty;
        public string ClassName { get; set; } = ""; 
        public int StudentCount { get; set; } 
    }
    public class StatusDistributionDto { 
        public string Status { get; set; } = ""; 
        public int Count { get; set; } 
        public double Percentage { get; set; } 
    }
    public class PaymentStatusCountDto { 
        public string Status { get; set; } = ""; 
        public int Count { get; set; } 
        public decimal Total { get; set; } 
    }
}
