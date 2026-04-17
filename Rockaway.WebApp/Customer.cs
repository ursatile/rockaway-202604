namespace Rockaway.WebApp;

public class Customer(string firstName, string lastName) {
	public string FirstName { get; set; } = firstName;
	public string LastName { get; set; } = lastName;
}