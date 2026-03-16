namespace ToRefactor
{
    internal class Program {
        static async Task Main(string[] args) {
            await ReportService.GenerateReportAsync();
        }
    }
}
