My Code's short description:
Controller Setup: We've created a controller named CountryGwpController that handles HTTP POST requests to the endpoint /api/gwp/avg.
**Parsing JSON Input**: The controller expects a JSON input in the format:
{
    "country": "ae",
    "lob": ["property", "transport"]
}
It checks if the required fields (country and lob) are present in the JSON payload.
**Reading CSV Data**: It reads data from a CSV file named gwpByCountry.csv located in the Data folder.
**Data Processing:**
It filters the CSV data based on the provided country.
It extracts information related to the specified lineOfBusiness (from the JSON input) from the CSV file.
Calculates the average of the values associated with each line of business mentioned in the JSON input.
**Generating Response:**
Constructs a JSON response containing the average values for the specified line of business.
Returns this JSON response with the calculated average values for each line of business mentioned in the input JSON.
**Error Handling:**
Provides error handling to catch exceptions that might occur during the data retrieval or processing.
Returns appropriate error messages and HTTP status codes if an error occurs.
