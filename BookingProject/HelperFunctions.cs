using System.Globalization;

namespace BookingProject.Controllers;

public  class HelperFunctions(ILogger logger)
{
    public  Tuple<DateOnly, DateOnly>? ParseCheckInOutDates(string checkInDate, string checkOutDate)
    {
        if (!DateOnly.TryParseExact(checkInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedCheckInDate) ||
            !DateOnly.TryParseExact(checkOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedCheckOutDate))
        {
            logger.LogError("HelperFunctions-ParseCheckInOutDates: Invalid dates format. " +
                              " Dates must use format yyyy-MM-dd, for example: /rooms?checkInDate=2026-06-20&checkOutDate=2026-06-22");
            return null;
        }
        return new Tuple<DateOnly, DateOnly>(parsedCheckInDate, parsedCheckOutDate);
    }
}