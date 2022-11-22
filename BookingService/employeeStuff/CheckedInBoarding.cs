using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookingService;

public class CheckedInBoarding
{
    public CheckedInBoarding(Boarding boarding, string email)
    {
        Boarding = boarding;
        Email = email;
        _id = ObjectId.GenerateNewId();
        BoardingId = boarding._id.ToString();
        
        int diffDays = boarding.EndDate.Subtract(boarding.StartDate).Days;
        BoardingTasks = new List<TaskToDo>();
        
        BoardingTasks.AddRange(FirstDayTasks(MakeTaskListForOneDay(), Boarding.DropOffTime));

        while (diffDays >= 1)
        {
            BoardingTasks.AddRange(MakeTaskListForOneDay());
            diffDays--;
        }
        
        BoardingTasks.AddRange(LastDayTasks(MakeTaskListForOneDay(), Boarding.PickUpTime));
    }

    [BsonId] public ObjectId _id { get; set; }
    [BsonElement("Boarding")] public Boarding Boarding { get; set; }
    [BsonElement("Email")] public string Email { get; set; }
    [BsonElement("BoardingTasks")] public List<TaskToDo> BoardingTasks { get; set; }
    [BsonElement("BoardingId")] public string BoardingId { get; set; }

    public List<TaskToDo> FirstDayTasks(List<TaskToDo> list, int startingTime)
    {
        var listToReturn = new List<TaskToDo>();
        listToReturn.Add(new TaskToDo(Boarding._id.ToString(), "Check in", 
            "Place the animal in the right enclosure make sure to check them in in the system", 
            Boarding.DropOffTime, 0));
        
        if (startingTime is >= 6 and <= 8) //start with feeding
        {
            listToReturn.AddRange(list.GetRange(1, list.Count - 1));
        }
        else if (startingTime is 9 or 10) //start with letting them out
        {
            listToReturn.AddRange(list.GetRange(2, list.Count - 2));
        }
        else if (startingTime is >= 11 and <= 14) //start with 2pm feeding
        {
            listToReturn.AddRange(list.GetRange(5, list.Count - 5));
        }
        else//start with 7pm feeding
        {
            listToReturn.AddRange(list.GetRange(8, list.Count - 8));
        }
        return listToReturn;
    }

    public List<TaskToDo> LastDayTasks(List<TaskToDo> list, int endingTime)
    {
        //latest ending time is 21
        var listToReturn = new List<TaskToDo>();
        if (endingTime is >= 8 and <= 9)
        {
            listToReturn.AddRange(list.GetRange(0, 2));
        }
        else if (endingTime is >= 10 and <= 14) 
        {
            listToReturn.AddRange(list.GetRange(0, 4));
        }
        else if (endingTime is >= 15 and <= 19) //put in cages 
        {
            listToReturn.AddRange(list.GetRange(0, 7));
        }
        else if (endingTime is 20 or 21) 
        {
            listToReturn.AddRange(list.GetRange(0, 9));
        }
        listToReturn.Add(new TaskToDo(Boarding._id.ToString(), "Check out", 
            "Get the animal ready for pick up, make sure to check them out in the system", 
            Boarding.PickUpTime, 0));
        return listToReturn;
    }
    
    public List<TaskToDo> MakeTaskListForOneDay() //each list is 11 in length
    {
        //7 am check up on all animals 
        //8 am start with feeding
        //10 am let the ones out that need to go out
        //12 clean up after feeding and anything else 
        //2 pm feed again
        //3 pm place some in cages 
        //5 pm let them out if they want to 
        //7 pm feed again
        //8 pm clean up after feeding and anything else
        //10 pm put everyone in cages 

        var returnList = new List<TaskToDo>();

        switch (Boarding.AnimalType)
        {
            case "Dog":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the dog and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the dog", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Let out",
                    "Let the dog out to the play area where they can go to the bathroom and play after", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the dog and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the dogs kennel and make sure the dog is clean", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the dog", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Place in kennel",
                    "Place the dog in their kennel for some quiet time", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Let out to play",
                    "Let the dog out to the play area if they seem ready if not reschedule", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the dog", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the dogs kennel and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Put in kennel",
                    "Put the dog in their kennel for the night", 22, 0));
                break;
            case "Cat":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the cat and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the cat", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Let out",
                    "Let the cat out to the play area where they can go to the bathroom and play after", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the cat and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cats room and don't forget about litter box", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the cat", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Place in room",
                    "Place the cat in their room for some quiet time", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Let out to play",
                    "Let the cat out to the play area if they seem ready if not reschedule", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the cat", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cats room and litter box make sure its all ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Put in room",
                    "Put the cat in their room for the night", 22, 0));
                break;
            case "Parrot":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the parrot and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the parrot", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the parrot",
                    "Give some attention to the parrot by holding it or talking to it", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the parrot and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the parrot", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the parrot doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the parrot and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the parrot", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Cover cage",
                    "Put a cover over the cage to make sure the parrot gets a full night of sleep", 22, 0));
                break;
            case "Snake":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the snake and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food if needed and water to the snake", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the snake",
                    "Give some attention to the snake by holding it or some other activity", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the snake and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food if needed and water to the snake", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the snake doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the snake and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food if needed and water to the snake", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage is in the right conditions for the particular snake", 22, 0));
                break;
            case "Tortoise":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the tortoise and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the tortoise", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the tortoise",
                    "Give some attention to the tortoise by holding it or letting it play in water", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the tortoise and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the enclosure of any old food or other dirt that shouldn't be in there", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the tortoise", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the tortoise doesn't need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the tortoise and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the tortoise", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Close the night enclosure",
                    "Make sure all tortoise are inside and close the night room where the tortoise can sleep", 
                    22, 0));
                break;
            case "Turtle":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the turtle and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food to the turtle", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the turtle",
                    "Give some attention to the turtle by holding it or letting it run around a bit", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the turtle and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food to the turtle", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the turtle doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change environment",
                    "Change some rocks and plants around so the turtle has new things to explore", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food to the turtle", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the turtle", 22, 0));
                break;
            case "Lizard":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the lizard and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the lizard", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the lizard",
                    "Give some attention to the lizard by holding it or letting it run around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the lizard and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the lizard", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the lizard doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the lizard and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the lizard", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the lizard", 22, 0));
                break;
            case "Hamster":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the hamster and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the hamster", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the hamster",
                    "Give some attention to the hamster by holding it or letting it run around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the hamster and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the hamster", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the hamster doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the hamster and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the hamster", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the hamster", 22, 0));
                break;
            case "Ferret":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the ferret and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the ferret", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the ferret",
                    "Give some attention to the ferret by holding it or letting it run around away from the rest", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the ferret and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the room from any garbage that shouldn't be there", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the ferret", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the ferret doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the ferret and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the ferret", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the room is correct for the night", 22, 0));
                break;
            case "Genuine Pig":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the genuine pig and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the genuine pig", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the genuine pig",
                    "Give some attention to the genuine pig by holding it or letting it run around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the genuine pig and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the genuine pig", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the genuine pig doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the genuine pig and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the genuine pig", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the genuine pig", 22, 0));
                break;
            case "Mice/Rat":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the animal and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the animal", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the animal",
                    "Give some attention to the animal by holding it or letting it run around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the animal and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage if its too bad get a new one and put the old one for cleaning", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the animal", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the animal doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the animal and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the animal", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the animal", 22, 0));
                break;
            case "Sugar Glider":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the sugar glider and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the sugar glider", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the sugar glider",
                    "Give some attention to the sugar glider by holding it or letting it climb and fly around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the sugar glider and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage make sure there is no way of escaping when closing it", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the sugar glider", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Play with sugar glider",
                    "Let the sugar glider fly around keep a close eye on it", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the sugar glider and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the sugar glider", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the sugar glider", 22, 0));
                break;
            case "Rabbit":
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Check up on the rabbit and make sure everything was good over the night", 7, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the rabbit", 8, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Entertain the rabbit",
                    "Give some attention to the rabbit by holding it or letting it run around", 10, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the rabbit and take some old ones away", 11, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up",
                    "Clean the cage get rid of any big messes and any old food", 12, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the rabbit", 14, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Check up",
                    "Make sure everything is okay and the rabbit doesnt need anything", 15, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Change toys",
                    "Give some new toys to the rabbit and take some old ones away", 17, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Feed",
                    "Give food and water to the rabbit", 19, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Clean up and get ready for bed",
                    "Clean the cage and make sure its ready for the night", 20, 0));
                returnList.Add(new TaskToDo(Boarding._id.ToString(), "Night check",
                    "Make sure the cage environment is correct for the rabbit", 22, 0));
                break;
        }
        return returnList;
    }

}