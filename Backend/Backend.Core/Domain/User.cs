
using Backend.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend.Core.Domain
{
    public class User
    {
        private ISet<UserProduct> _products = new HashSet<UserProduct>();
        private ISet<UserExercise> _exercises = new HashSet<UserExercise>();

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Role Role { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<UserProduct> Products
        { 
            get { return _products; }
            set { _products = new HashSet<UserProduct>(value); }
        }

        public IEnumerable<UserExercise> Exercises
        {
            get { return _exercises; }
            set { _exercises = new HashSet<UserExercise>(value); }
        }

        public User()
        {

        }

        public User(string username, string email, string password, string salt, Role role)
        {
            SetUsername(username);
            SetEmail(email);
            SetPassword(password, salt);
            CreatedAt = DateTime.Now;
            Role = role;
            UpdatedAt = DateTime.Now;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException(ErrorCodes.InvalidUsername, "Username cannot be empty.");
            }

            Username = username.ToLowerInvariant();
            UpdatedAt = DateTime.Now;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException(ErrorCodes.InvalidEmail, "Email cannot be empty.");
            }

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.Now;
        }


        public void SetPassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException(ErrorCodes.InvalidPassword, "Password cannot be empty.");
            }

            if (password.Length < 4)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password cannot contain less than 4 characters.");
            }

            if (password.Length > 100)
            {
                throw new DomainException(ErrorCodes.InvalidPassword,
                    "Password cannot contain more than 100 characters.");
            }

            if (Password == password)
                return;

            Password = password;
            Salt = salt;
            UpdatedAt = DateTime.Now;
        }

        public void AddProduct(User user, Product product, double weight)
        {

            _products.Add(UserProduct.Create(user, product, weight));
            UpdatedAt = DateTime.Now;
        }


        public void AddExercise(User user, Exercise exercise, double weight, int numberOfSets,
           int numberOfReps, Day day)
        {
            _exercises.Add(UserExercise.Create(user, exercise, weight, numberOfSets,
                numberOfReps, day));

            UpdatedAt = DateTime.Now;
        }

        public void DeleteExercise(User user, Exercise exercise)
        {
            var userExercise = Exercises.SingleOrDefault(x => x.UserId == user.Id &&
            x.ExerciseId == exercise.Id);
            if (userExercise == null)
            {
                throw new DomainException(ErrorCodes.ObjectNotFound, $"Exercise with id: '{exercise.Id}' " +
                    $"for user with id: '{user.Id}' was not found");
            }
        }

    }
}
