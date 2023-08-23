# LibHUB
 A project I created using Visual Studios .Net Core Framework, Entity Framework, Blazor WebAssemply, and Web APIs which is a library database management system including features such as borrow, user, and book database administration and user informing systems. <br />
 <br />
This web application comes with a number of policies such as: 
- A user can borrow a maximum of 5 books
- A user can renew a book no more than 5 times
- A user cannot borrow or renew books if they have any overdue books
- A user can only rate a book when they return the book and they have never rated the book before<br />
  <br />
These policies are applied when restricting a user's ability to borrow or renew a book by not making these features accessible until they resolve these policies if they do not follow them. In addition, the application will provide an alert within the user's profile to inform as to why the user cannot use certain features at a given time.<br />
<br />
Finally, you can also notify all users with overdue books when pressing the "Notify All" or "Notify" buttons available on the "Users with Fees Fined" page and "Users With Overdue Books But No Fines" page.<br />
<br />
<img width="901" alt="HomeScreen" src="https://github.com/AmbikaiSasitharan/LibHUB/assets/140208293/f5c94bee-c985-4a4e-9ea0-ce6ad1db3aac">
<img width="901" alt="UserListPage" src="https://github.com/AmbikaiSasitharan/LibHUB/assets/140208293/a02ba65b-353f-4dfb-affc-d07f9d5637df">
<img width="901" alt="UserProfileInformationPage" src="https://github.com/AmbikaiSasitharan/LibHUB/assets/140208293/61331967-198e-4546-b21a-532f913e0bdf">
<img width="901" alt="BookListPage" src="https://github.com/AmbikaiSasitharan/LibHUB/assets/140208293/1a202df8-5b77-4469-bd50-2b76830ea543">
<img width="901" alt="BookProfileInformationPage" src="https://github.com/AmbikaiSasitharan/LibHUB/assets/140208293/a812de08-6333-4b66-a13b-cb0f78ef199e">

In the future I would like to add more features such as: 
- Integration of PayPal Payment Gateway that will appear when a user tries to return a book that has a fine on it
- Adding an Add Cart DTO to allow users to browse for books and maintain a list to dynamically change before confirming the borrow of multiple books in one transaction
- and more...

If you would like to use this code and practice it urself you will have to: 
1. Download the LibHub.Api, LibuHub.Web, and LibHub.Models files.
2. Create a new project in Visual Studios.
3. Add the three files as Existing Items into your newly created project solution.
4. Replace the  "EmailHost","EmailUserName","EmailPassword", "ConnectionStrings" values in the appsettings.json found under the LibHub.Api folder with yoru libraries values.
 
