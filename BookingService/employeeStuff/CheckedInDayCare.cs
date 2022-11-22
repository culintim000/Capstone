using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class CheckedInDayCare
{
    public CheckedInDayCare(Daycare daycare, string email)
    {
        _id = ObjectId.GenerateNewId();
        Daycare = daycare;
        Email = email;
        DaycareId = daycare._id.ToString();

        int totalHours = daycare.PickUpTime - daycare.DropOffTime;

        DayCareTasks = makeTaskList(totalHours);
    }


    [BsonId] public ObjectId _id { get; set; }
    [BsonElement("Daycare")] public Daycare Daycare { get; set; }
    [BsonElement("Email")] public string Email { get; set; }
    [BsonElement("DayCareTasks")] public List<TaskToDo> DayCareTasks { get; set; }
    [BsonElement("DaycareId")] public string DaycareId { get; set; }


    public List<TaskToDo> makeTaskList(int totalHours)
    {
        var returnList = new List<TaskToDo>();

        switch (Daycare.AnimalType)
        {
            case "Dog":
                returnList.Add(new TaskToDo(Daycare._id.ToString().ToString(), "Place in Play Area",
                    "Let the dog run around and play with other dogs", Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the animal",
                    Daycare.DropOffTime + 1, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the animal",
                    Daycare.DropOffTime + 1, 0));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on animal to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys",
                        "Give some new toys to the animal and take away any old ones",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Transfer to Quite Room",
                        "Put the animal in a quite room where they can take a nap",
                        Daycare.DropOffTime + 9, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the animal",
                        Daycare.DropOffTime + 8, 30));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Let into Play Area",
                        "Let the animal into the play area if they seem ready",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Cat":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Social Room", "Place the cat in the social room",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the cat",
                    Daycare.DropOffTime + 1, 0));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the cat",
                    Daycare.DropOffTime + 1, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the cat to see if it needs anything, play with it, if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys",
                        "Give some new toys to the cat and take away any old ones",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the animal",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Transfer to Quite Room",
                        "Put the cat in a quite room where they can take a nap",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Let into Social Area",
                        "Let the animal into the social room if they seem ready",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Parrot":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the parrot in a cage that is the correct size",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the parrot",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the parrot",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the parrot to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys",
                        "Give some new toys to the parrot and take away any old ones",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water", "Add food and water into the cage",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the parrot or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the parrot to see if it needs anything, play with it if it seems to need it",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Snake":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the snake in a cage that is the correct size make sure its warm enough with the right humidity",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the snake",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the snake to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and the humidity is correct",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the snake needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the snake or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the snake to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Tortoise":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the tortoise in a cage that is the correct size make sure its warm enough",
                    Daycare.DropOffTime + 0, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the tortoise",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the tortoise",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the tortoise to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and the humidity is correct",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the tortoise needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the tortoise or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the tortoise to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Turtle":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the turtle in a cage that is the correct size make sure its warm enough",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the turtle",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the turtle to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the water is warm enough and the humidity is correct",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the turtle needs it",
                        Daycare.DropOffTime + 8, 30));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the turtle to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Lizard":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the lizard in a cage that is the correct size make sure its warm enough",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the lizard",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the lizard",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the lizard to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and the humidity is correct",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the lizard needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the lizard or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the lizard to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Hamster":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the hamster in a cage that is the correct size make sure its clean",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the hamster",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the hamster",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the hamster to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the hamster needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the hamster or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the hamster to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Ferret":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Big Cage",
                    "Place the ferret in the cage with the other ferrets",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the ferret",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the ferret",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the ferret to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the ferret or entertain it in some way",
                        Daycare.DropOffTime + 5, 0));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the ferret needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the ferret or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the ferret to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Genuine Pig":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the genuine pig in a cage that is the correct size make sure its clean",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the genuine pig",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the genuine pig",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the genuine pig to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the genuine pig needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the genuine pig or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the genuine pig to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Mice/Rat":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Cage",
                    "Place the animal in a cage that is the correct size make sure its clean",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the animal",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the animal",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the animal to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the animal needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the animal or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the animal to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Sugar Glider":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Big Cage",
                    "Place the sugar glider in the cage with the other sugar gliders",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the sugar glider",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the sugar glider",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the sugar glider to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the sugar glider or entertain it in some way",
                        Daycare.DropOffTime + 5, 0));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the sugar glider needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the sugar glider or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the sugar glider to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
            case "Rabbit":
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Place in Big Cage",
                    "Place the rabbit in the cage with the other rabbit",
                    Daycare.DropOffTime, 5));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Water", "Give Water to the rabbit",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Give Food", "Give Food to the rabbit",
                    Daycare.DropOffTime, 30));
                returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                    "Check up on the rabbit to see if it needs anything, play with it if it seems to need it",
                    Daycare.DropOffTime + 2, 0));

                if (totalHours >= 4)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Change Toys", "Change the toys in the cage",
                        Daycare.DropOffTime + 4, 0));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check on Conditions",
                        "Make sure the cage is warm enough and clean",
                        Daycare.DropOffTime + 4, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the rabbit or entertain it in some way",
                        Daycare.DropOffTime + 5, 0));
                }

                if (totalHours >= 8)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Add Food and Water",
                        "Add food and water if the rabbit needs it",
                        Daycare.DropOffTime + 8, 30));
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Play/Entertain",
                        "Play with the rabbit or entertain it in some way",
                        Daycare.DropOffTime + 9, 0));
                }

                if (totalHours >= 12)
                {
                    returnList.Add(new TaskToDo(Daycare._id.ToString(), "Check Up",
                        "Check up on the rabbit to see if it needs anything",
                        Daycare.DropOffTime + 12, 0));
                }

                break;
        }

        returnList.Add(new TaskToDo(Daycare._id.ToString(), "Get Ready for Checkout", "Get the animal ready for checkout",
            Daycare.DropOffTime + totalHours - 1, 45));


        //order the list by time
        return returnList.OrderBy(d => d.StartHour.ToString().Length).ThenBy(d => d.StartHour)
            .ThenBy(d => d.StartMinute.ToString().Length).ThenBy(d => d.StartMinute).ToList();
    }
}