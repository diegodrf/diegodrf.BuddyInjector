using System.Text;

namespace diegodrf.BuddyInjector.Exceptions;

public class MultipleConstructorsException : Exception
{
    public MultipleConstructorsException(string? message = null) : base(message) { }
}