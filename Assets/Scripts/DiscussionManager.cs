using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discussion {
	public Neighbor neighbor;
	public string key;
	public string introduction;

	public List<Response> responses;

	public Discussion(Neighbor n, string k, string intro, List<Response> resps) {
		neighbor = n;
		key = k;
		introduction = intro;
		responses = resps;

		for(int i = 0; i < responses.Count; i++) {
			responses[i].key = k;
		}
	}
}

public enum ResponseType {
	Environment,
	Money,
	Technology,
	Accept,
	Greet,
	Wind,
	Solar,
	Biomass
}

public class Response {
	public string key;
	public ResponseType type;
	public string response;
	public string reaction;
	
	public bool successful = true;


	public Response(ResponseType t, string resp, string react) {
		type = t;
		response = resp;
		reaction = react;
	}

	public Response(ResponseType t, string resp, string react, bool success) {
		type = t;
		response = resp;
		reaction = react;

		successful = success;
	}
}

public class DiscussionManager : MonoBehaviour {

	private GameManager gameManager;

	public List<Discussion> discussions = new List<Discussion>();
	public List<Discussion> solarInstallationDiscussions = new List<Discussion>();

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	public void SetDiscussions() {
		discussions.Add(new Discussion(GameManager.player, "instruction", "Hey, neighbor! Welcome to the neighborhood. My name is Erica and I work for the local power company. All of the homes on this block come with some pretty standard appliances. Feel free to upgrade them. This will reduce your energy use and save you money on your monthly bill.", 
		new List<Response> {
			new Response(ResponseType.Greet, "You introduce yourself to Erica and tell her it's nice to meet her.", "Our long term goal here at the power company is to produce our own energy. The faster we can do that the better! Of course, first you should focus on reducing your own energy use. Good luck!")
		} ));

		discussions.Add(new Discussion(GameManager.player, "promotion", "Hello, neighbor. We're looking for someone to take on the role of Smart Grid Manager. We think that you'd be a great fit! All you need to do is help your neighbors reduce their household energy use. Do you accept the position?", 
		new List<Response> {
			new Response(ResponseType.Accept, "You accept the the offer. Starting next month, you'll receive $500 every month.", "Great! You should start working with your neighbors when you're not busy at home. You might figure out what motivates them.  Usually it's money, the environment, or new technology. Best of luck!")
		} ));

		/*
		Neighbor agreeableNeighbor = gameManager.neighborList[0];
		SetNeighborETM(agreeableNeighbor);
		*/

		Neighbor environmentalNeighbor = gameManager.neighborList[0];
		SetNeighborE(environmentalNeighbor);

		Neighbor techMoneyNeighbor = gameManager.neighborList[1];
		SetNeighborTM(techMoneyNeighbor);

		Neighbor techNeighbor = gameManager.neighborList[2];
		SetNeighborT(techNeighbor);

		Neighbor environmentMoneyNeighbor = gameManager.neighborList[3];
		SetNeighborEM(environmentMoneyNeighbor);

		discussions.Add(new Discussion(GameManager.player, "startConstruction", "Heya! Your neighbors say you're doing a great job reducing the neighborhood's energy use. Our last goal is to generate all our own energy. We can do that by constructing a wind, solar, or leaf-litter biomass farm. Which of those would you prefer?",
		new List<Response> {
			new Response(ResponseType.Wind, "You agree to work towards building a wind farm. Once you've helped your neighbors enough, they'll likely help you.", "That's great! You can begin building any time, but you can build faster if more neighbors contribute. You can also afford it sooner if you reduce your monthly energy costs. Best of luck! And thank you for helping everyone!"),
			new Response(ResponseType.Solar, "You agree to work towards building a solar farm. Once you've helped your neighbors enough, they'll likely help you.", "That's great! You can begin building any time, but you can build faster if more neighbors contribute. You can also afford it sooner if you reduce your monthly energy costs. Best of luck! And thank you for helping everyone!"),
			new Response(ResponseType.Biomass, "You agree to work towards building a leaf-litter biomass farm. Once you've helped your neighbors enough, they'll likely help you.", "That's great! You can begin building any time, but you can build faster if more neighbors contribute. You can also afford it sooner if you reduce your monthly energy costs. Best of luck! And thank you for helping everyone!")
		}));
	}

	public void AddFinalDiscussion(string type) {
		if(type == "solar") {
			discussions.Add(new Discussion(GameManager.player, "endGame", "Great job! All of your neighbors have pitched in to fund the solar farm construction! If you're ready to pitch in as well, we can begin building right away!",
			new List<Response> {
				new Response(ResponseType.Accept, "You tell Erica that once you're ready you'll tell the construction workers outside your house to begin construction.", "Fantastic! Your solar panels should last 40 years or more, with very little upkeep. Solar energy is a great source. Most of the earth's energy comes from the sun. Solar panels are a great, clean source of energy!")
			}));
		} else if(type == "wind") {
			discussions.Add(new Discussion(GameManager.player, "endGame", "Great job! All of your neighbors have pitched in to fund the wind farm construction! If you're ready to pitch in as well, we can begin building right away!",
			new List<Response> {
				new Response(ResponseType.Accept, "You tell Erica that once you're ready you'll tell the construction workers outside your house to begin construction.", "Fantastic! Your wind turbines should last 20 years or more. There are some costs to keep them working. Fortunately the money you save on energy will make up for the cost of upkeep!")
			}));
		} else if (type == "biomass") {
			discussions.Add(new Discussion(GameManager.player, "endGame", "Great job! All of your neighbors have pitched in to fund the leaf-litter biomass farm construction! If you're ready to pitch in as well, we can begin building right away!",
			new List<Response> {
				new Response(ResponseType.Accept, "You tell Erica that once you're ready you'll tell the construction workers outside your house to begin construction.", "Fantastic! Coal comes from plants which lived long, long ago. Biomass power plants use energy from recently-living plants instead. Your community will be able to turn leaves, wood, paper, and other wastes into energy!")
			}));
		} else {
			Debug.Log("no existo contructo");
		}
	}	


	//Need to figure out how to incorporate this
	public void AddConvincingDiscussions(string farmTypeString, Neighbor neighbor) {
		if(neighbor == gameManager.neighborList[0]) { //Leon
			discussions.Add(new Discussion(neighbor, "convince", "Hey the Smart Grid Company sent me an email saying that they want to build a " + farmTypeString + ". You've helped me a lot since you've moved in. Do you have any thoughts on why that might be a good idea?",
			new List<Response> {
				new Response(ResponseType.Environment, "You explain that this project would allow him to produce green energy.  We can help the earth by using fewer fossil fuels.", "You really are good at your job. Alright, I'll help fund the construction. You know I can't say no to helping the environment! Thanks for everything, friend.", true),
				new Response(ResponseType.Money, "You explain that the " + farmTypeString + " will pay for itself in about ten years. The neighborhood won't have to buy energy from the grid.", "Is that the ice cream truck I hear? Let's talk another time.", false),
				new Response(ResponseType.Technology, "You explain that this project uses advanced energy-effecient technology.", "Friend, you should know by now that I don't care about tech. Let's talk another time.", false)
			} ));
		} else if(neighbor == gameManager.neighborList[1]) { //Amina
			discussions.Add(new Discussion(neighbor, "convince", "Hey friend! The Smart Grid Company sent out a letter saying they want to build a " + farmTypeString + ". They want my help funding it but it seems oh so expensive! Why do you think I should invest my money into this project?",
			new List<Response> {
				new Response(ResponseType.Environment, "You tell Amina that this project would help the earth. It allows us to produce our own green energy. This means less use of fossil fuels!", "That makes sense to me. But I'm afraid I just can't afford to help save the earth right now. I've already spent so much to improve my energy efficiency. Maybe you could come by another time to talk about this.", false),
				new Response(ResponseType.Money, "You tell her that this project will save her money in the long run. We won't need to buy power from the national grid. Instead, we can sell energy to the grid for a profit!", "I do like the sound of that! It's quite a high price, but if we won't have to pay for our power anymore I'm on board! Thank you for everything, friend.", true),
				new Response(ResponseType.Technology, "You explain to Amina that this " + farmTypeString + " is using some of the most advanced tech. It will allow us to produce our own green energy.  And not just this year -- for decades!", "That sounds fascinating! I'd love to see this technology put to work! Thanks for all of your advice, neighbor.", true)
			} ));
		} else if(neighbor == gameManager.neighborList[2]) { //Terrance
			discussions.Add(new Discussion(neighbor, "convince", "Hello there. I hear the power company wants to construct a " + farmTypeString + ". That sounds pretty interesting to me but do you think I should help fund it?",
			new List<Response> {
				new Response(ResponseType.Environment, "You explain that it would allow us to produce nearly all our energy. We wouldn't need to use as many fossil fuels.", "Hmm. Y'know, I already have those solar panels. I'm producing quite a lot of energy myself. Maybe we can talk about this some other time?", false),
				new Response(ResponseType.Money, "You explain that it would save each neighbor a lot of money. It will produce enough energy to offset the monthly energy costs.", "Haven't I told you that money isn't a concern of mine? I'm not sure making a long-term investment like this is in my best interest. Maybe you can stop by some other time.", false),
				new Response(ResponseType.Technology, "You explain that this project uses the newest green energy tech. It will provide energy for decades!", "The thought of being able to produce that much energy is great. I'll help pitch in. By the way, you've done an excellent job helping me out, neighbor. Thank you!", true)
			} ));
		} else if(neighbor == gameManager.neighborList[3]) {
			discussions.Add(new Discussion(neighbor, "convince", "Hello, friend. I got a letter from the power company telling me they want help building a " + farmTypeString + ". Why would I want to spend so much on something like that? Haven't I done enough already?",
			new List<Response> {
				new Response(ResponseType.Environment, "You thank Gerald for his improved energy habits. With a <energy farm> we can change everyone’s habits. Producing green energy will reduce our use of fossil fuels.", "Hmph. You always gotta be right, huh? Alright, alright I'll help pay for the thing. But this is the last thing I'm doing, I swear! Anyways, I've appreciated your help, youngster.", true),
				new Response(ResponseType.Money, "You tell Gerald that the " + farmTypeString + " should produce lots of energy. He may no longer need to pay for energy!", "Is that so? Well you haven't let me down yet so I'll take your word for it. I'll send in my check. I truly appreciate the advice you've given me since you moved in, friend.", true),
				new Response(ResponseType.Technology, "You explain that the " + farmTypeString + " will use some of the most advanced tech. You begin listing the pros and cons until you notice Gerald asleep in the doorway.", "Huh? Did I nod off? You'll have to excuse me, it's time for my post-lunch nap. We can talk about that some other time, yeah?", false)
			} ));
		}
	}

	/*
	discussions.Add(new Discussion(neighbor, "", "",
		new List<Response> {
			new Response(ResponseType.Environment, "", ""),
			new Response(ResponseType.Money, "", ""),
			new Response(ResponseType.Technology, "", "")
	} ));
	*/

	//These have all been removed.
	/*
	public void SetNeighborETM(Neighbor neighbor) {
		discussions.Add(new Discussion(neighbor, "introduction", "Hey there neighbor my name is Erica. I heard you've agreed to become the Smart Grid Manager for our neighborhood and I look forward to hearing your thoughts on how I can improve my energy use.",
		new List<Response> { 
			new Response(ResponseType.Greet, "You introduce yourself to Erica.", "It's nice to meet you, neighbor! Come back sometime next month and I'm sure I'll need some advice on one thing or another.")
		} ));
	}
	*/

	public void SetNeighborE(Neighbor neighbor) {
		discussions.Add(new Discussion(neighbor, "introduction", "Sup. My name's Leon. I heard you can help me make decisions that'll be good for the environment.",
		new List<Response> { 
			new Response(ResponseType.Greet, "You introduce yourself and assure Leon that you'll do your best.", 
			"That's good to hear. Looking forward to getting your advice next time you stop by.")
		} ));

		discussions.Add(new Discussion(neighbor, "dryerRack", "Hey, neighbor. The power company sent out an email saying that I should use my dryer less. Why would I wanna do that?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that a drying rack can be better than a dryer. Hanging clothes to dry uses no energy. Hanging clothes has less of an impact on the Earth.", 
			"That makes so much sense! I can just stop using my dryer. Thanks for your help, friend.", true),
			new Response(ResponseType.Money, "You explain that he can dry clothes on a drying rack instead of a dryer. He can save a lot of money by letting his clothes air dry.", 
			"I guess that makes sense, but I'm not really concerned about money. I'm more interested in benefiting the Earth. Maybe you could stop by again some other time.", false),
			new Response(ResponseType.Technology, "You explain that instead of using a dryer, he could hang clothes. You mention that the latest drying racks look really cool. Some even have cup holders!", 
			"Ummm why would I need a cup holder while I'm drying my clothes? I don't think I understand the point. Thanks for stopping by, though.", false)
		} ));

		discussions.Add(new Discussion(neighbor, "ledLights", "Hey. Do you know what type of lights I should get? I've been using those curly looking CFLs.  Someone told me they were environmentally friendly.",
		new List<Response> {
			new Response(ResponseType.Environment, "You tell Leon that the average CFL uses 14 watts while an LED uses 10. LED bulbs are actually the best for the Earth.", 
			"Wow, I didn't know that! Thanks for teaching me new things, friend. Next time I'm at the store I'll get some LEDs.", true),
			new Response(ResponseType.Money, "You explain that while LEDs cost the most, they use less energy and last much longer. If you look long-term, they're the cheapest light bulb!", 
			"Huh. I’m not really worried about money, but if it's good for the Earth I’ll think a little more about it. Feel free to stop by some other time, neighbor.", false),
			new Response(ResponseType.Technology, "You explain to Leon that LEDs are the most advanced light bulbs. They're small, efficient, and they look really cool.", 
			"I don't care about looking cool. I'm only interested in saving the Earth. I guess I'll just stick with the CFLs. Feel free to come back some other time, neighbor.", false)
		} ));

		discussions.Add(new Discussion(neighbor, "smartThermostat", "Hey, neighbor. I could use your advice on some energy use stuff. When I'm not home I usually have my air conditioning on and it seems like it wastes a lot of energy. Is there any way to cut down on that?",
		new List<Response> {
			new Response(ResponseType.Environment, "You teach Leon about smart thermostats. They allow you to program and control your home’s heating and cooling. They make it easy for him to save energy and the earth when he is gone.", 
			"Perfect! That's exactly the type of thing I was looking for. Your advice is always appreciated, friend. See ya soon.", true),
			new Response(ResponseType.Money, "You teach Leon about smart thermostats.  They automatically control heating and cooling. He can save money by not heating or cooling an empty home.", 
			"Huh. I guess I don't fully understand how it works. I'm gotta go take a nap, but maybe you can explain it to me again some other day.", false),
			new Response(ResponseType.Technology, "You teach Leon about smart thermostats, which automatically adjust heating and cooling. He can let the system save energy for him, or he can control things from his phone.", 
			"That sounds a bit complicated and I'm not sure I understand. I'm headed out to volunteer, but maybe you can come back some other day and teach me more about it.", false)
		} ));


		discussions.Add(new Discussion(neighbor, "solar", "Hey there, friend. I've been thinking about getting solar panels lately. I know they’re pricey and I just lost hours at work. Do you think I should get some?",
		new List<Response> {
			new Response(ResponseType.Environment, "You tell Leon that solar panels could help save the Earth. They capture energy from the sun, which is better than burning fossil fuels.", 
			"Wow.  That sounds great, but even with that kind of impact I don’t know if I can afford it right now. Hopefully things will pick up at work soon.", false),
			new Response(ResponseType.Money, "You tell him solar panels are a great way to save money. They reduce the amount of energy you need to buy from the power grid.", 
			"That’s great news.  I can save money and the Earth at the same time. Thanks, it was nice talking to you again!", true),
			new Response(ResponseType.Technology, "You tell Leon that solar panels have improved in recent years. An average solar panel can light 120 LED bulbs for an hour a day. That's a lot of light!", 
			"Weird, but okay. I'm not sure why I'd want to have 120 lights going at the same time. Does my house look like it needs that many lights? Let's talk more about solar energy some other time.", false)
		} ));
	}

	public void SetNeighborTM(Neighbor neighbor) {
		discussions.Add(new Discussion(neighbor, "introduction", "Oh you must be the new Smart Grid Manager! I'm so excited to hear what new gadgets I can use to save money! My name is Amina, by the way.",
		new List<Response> { 
			new Response(ResponseType.Greet, "You introduce yourself to Amina and tell her it's nice to meet her.", "Nice to meet you, too. I'm sure I'll be seeing you around very soon!")
		} ));

		discussions.Add(new Discussion(neighbor, "upgradingAppliances", "Good to see you again, neighbor. I heard that you've been buying upgraded appliances recently. Why is that? Should I upgrade my appliances too?",
		new List<Response> {
			new Response(ResponseType.Environment, "You tell Amina you've been replacing old appliances with more efficient ones. They use less energy and are better for the Earth.", 
			"Oh, okay. Well I'm not too concerned about that right now but good for you! You seem like a smart kid. I've gotta run some errands now so maybe we can talk more about that later.", false),
			new Response(ResponseType.Money, "You tell her you've been replacing your old appliances with more efficient ones. They use less energy and save money on your energy bill.", 
			"I hadn't thought of it like that. Even though they are expensive, they save you money in the long run. Thank you for the advice. I'm gonna start looking for a new fridge right now!", true),
			new Response(ResponseType.Technology, "You tell her you love tech and to have energy saving appliances.", 
			"Ohh, I'm the same way! I simply adore seeing all the latest and greatest in technology. I'll start doing some research on what to buy straight away!", true)
		} ));

		discussions.Add(new Discussion(neighbor, "ledLights", "Hey, perfect timing! I'm about to run out and get some light bulbs.  What type do you think I should buy? I usually go with whatever’s cheapest.",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that LED bulbs are the best. They use the least energy and last the longest. As a result, they are best for the Earth!", 
			"A couple of light bulbs can't be that harmful, can they? I'm just going to buy the cheapest ones but thanks for stopping by.", false),
			new Response(ResponseType.Money, "You explain that LEDs use less energy and last much longer. While each bulb costs more, they save money in the long run.", 
			"Oh, I didn't know that! I just saw that they were more expensive and I didn't think about how costs add up. Thanks for the advice, neighbor. I'm off to the store!", true),
			new Response(ResponseType.Technology, "You explain that LED bulbs are the most advanced. They're small, efficient, and come in many colors.", 
			"I do really like how bright they are! The ones I have now sort of flicker sometimes. LEDs it is! Thanks for your advice, neighbor.", true)
		} ));

		discussions.Add(new Discussion(neighbor, "smartThermostat", "Hey! It's great to see you again! I've been looking over last year's energy bills and I noticed that heating has cost me a lot of money! Sometimes I forget to turn off the heat when I'm out of the house. Do you know how to prevent that?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that she should do her best.  If she isn’t going to be home, heating the air wastes energy and contributes to pollution.", 
			"Yeah I guess that makes sense. But I'm ever so forgetful at times. I wish there was some way to turn it off automatically. I hope you have a wonderful day, neighbor.", false),
			new Response(ResponseType.Money, "You explain that she can install a smart thermostat. This would automatically adjust the air in her home. When she's gone, she won't pay to heat an empty home.", 
			"Oh, that's perfect! With a smart thermostat I won't have to worry about remembering to turn off the heat. And the house can be comfortable by the time I get home. Thanks for stopping by, neighbor!", true),
			new Response(ResponseType.Technology, "You tell her she can install a smart thermostat. It turns the heat off when she is away from home.  It can adjust to many other preferences too!", 
			"Oh that sounds amazing! I always feel so hot at night. It'd be nice to have it set up to be a bit cooler when I'm sleeping. You always have the best advice, neighbor! Please keep stopping by!", true)
		} ));

		discussions.Add(new Discussion(neighbor, "offPeak", "Hey, neighbor! I got an email from the power company yesterday. They said I can save money by using electricity during something called off-peak hours. Can you explain what that means?",
		new List<Response> { 
			new Response(ResponseType.Money, "You explain that energy prices depend on use. Power is cheaper when fewer people are using it. You can save money by using appliances when other people aren't.", 
			"Oh, that's good to know! I'll check the power company's website and figure out the off-peak hours so I can save some money. Thanks for the info!", true),
			new Response(ResponseType.Technology, "You explain that power prices change based on how many people are using it. You can set appliances to run when prices are low.", 
			"Hmm.  That sounds complicated. I guess I'll have to look into setting that up another time. Thanks for your help!", false)
		} ));

		discussions.Add(new Discussion(neighbor, "windSell", "Hey, how's it going? I'm thinking about installing a wind turbine. I was reading something that said I could sell energy I don’t use back to the grid. Can you explain how that works or why I'd even do it?",
		new List<Response> {
			new Response(ResponseType.Money, "You tell Amina that wind turbines help save the earth by replacing fossil fuels with clean energy from wind.  Selling wind energy is just a bonus!", 
			"That’s a nice thought.  I’m not sure I can really have that kind of impact, though!  See you again soon.", false),
			new Response(ResponseType.Technology, "You explain that you can sell energy to the power grid. The power company will pay you if you generate more than you use.", 
			"So I can actually make money from the power company if I don't use too much power? I will have to look into a wind turbine! Thanks for stopping by, friend.", true)
		} ));
	}

	public void SetNeighborT(Neighbor neighbor) {
		discussions.Add(new Discussion(neighbor, "introduction", "Hey there. My name's Terrance. You must be one of the neighbors. What brings you by?",
		new List<Response> { 
			new Response(ResponseType.Greet, "You introduce yourself to Terrance. You explain you're the Smart Grid Manager. You also explain that your job is to help everyone make better energy choices.", "Oh, sweet! So you must know about some pretty cool new technology! It's nice to meet you and I look forward to seeing you around.")
		} ));

		discussions.Add(new Discussion(neighbor, "upgradingAppliances", "It's nice to see you again, neighbor. Word on the street is that you've been upgrading your appliances lately. I usually love tech, but I hear smart appliances might have security issues.  What have you found?",
		new List<Response> {
			new Response(ResponseType.Environment, "You tell Terrance that energy efficient appliances help the Earth. Less wasted energy means less pollution!", 
			"That’s a neat thought, but I’m still worried about security. Maybe you could come back some other time and we can talk about that technology!", false),
			new Response(ResponseType.Money, "You tell Terrance that using efficient appliances can save him money. His energy bill comes from his energy use.", 
			"Money isn't really an issue for me. I’m really wondering about security. Is it good to have a connected washer? Maybe I'll see you again soon.", false),
			new Response(ResponseType.Technology, "You tell him your appliances can only be controlled from within your home network.  You also joke about the risk of someone else doing laundry for you.", 
			"Hahaha good point! Thanks for the advice, neighbor. I'll have to start looking into some SmartTech appliances.", true)
		} ));

		discussions.Add(new Discussion(neighbor, "turnLightsOff", "Good afternoon! I could use your advice on something. My partner was telling me that I shouldn't leave my lights on during the day. We're both at work, but it's so easy to forget about them! I don't think it's a big deal anyways.",
		new List<Response> {
			new Response(ResponseType.Environment, "You tell him that leaving lights on wastes energy.  If you do it every day, it adds up!", 
			"I understand that. But I'm a busy man and I can't always run around the house turning off all the lights. If only there was some way to do it automatically. Oh well, thanks for the advice.", false),
			new Response(ResponseType.Money, "You let him know that leaving lights on can add up on his energy bill.", 
			"Money isn't really an issue. I try to turn them off when I can, but if I pay a few extra dollars that's okay. If only there was some way to shut them off automatically. Thanks for stopping by, neighbor.", false),
			new Response(ResponseType.Technology, "You inform him that he can have his lights connected to a smart sensor. His lights would turn off when there’s no one home.", 
			"Oh, that's brilliant! Your advice is always great. I'll look at getting a smart device like that installed right away.", true)
		} ));

		discussions.Add(new Discussion(neighbor, "unplug", "Hello. I was reading something online that said you're supposed to unplug appliances when you won't be using them for a long time. Is that true? Why would you do that?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that appliances still use power when they're off. While it may seem small, ‘vampire energy’ from things like TVs can add up.", 
			"That sounds like a lot of work just to be slightly helpful for the Earth. I can't go running around my house unplugging everything all the time. If only there was a simpler way!", false),
			new Response(ResponseType.Money, "You explain that appliances still use power when they're turned off. Unplugging them when you won't be using them for a while can save money.", 
			"I doubt the money I could save is worth the effort of unplugging everything when I leave town. Maybe I'd do it if there was a simpler way.", false),
			new Response(ResponseType.Technology, "You explain that appliances still use power when they're off. You can plug multiple devices into a power strip. That way they can be unplugged all at once.", 
			"Now that's a great idea! I've got a power strip in my home office but I could easily use one in my other rooms. That'd make this unplugging business much simpler. Thanks for the advice, neighbor.", true)
		} ));


		discussions.Add(new Discussion(neighbor, "solar", "Hey! It's good to see you. Do you have any thoughts on solar panels? My friend just had some installed and they fascinate me. Do you think now is a good time to get some?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that most power grids burn fossil fuels. Solar panels use energy from the sun.", 
			"Yes, yes, I know that they're green and all that. I plan to get them but I'm not sure if now is a good time. I've gotta run but let's talk more about this sometime.", false),
			new Response(ResponseType.Money, "You explain that solar panels would reduce the energy he needs to buy. Solar panels can pay for themselves in as little as 10 years.", 
			"To be honest I'm not sure if I'll be living in this house for that long, so I don't know if it'd be worth it. But thanks for coming by with your advice.", false),
			new Response(ResponseType.Technology, "You explain that solar tech has improved in recent years. 'Solar skins' even allow panels to blend in with the roof.", 
			"Ohh, now that's cool! I was concerned solar panels might look ugly. I always learn so much talking to you, my friend.", true)
		} ));

		discussions.Add(new Discussion(neighbor, "solarSell", "Nice to see you again. To follow up on solar panels, I hear that you can actually sell solar energy.  Is that seamless or does it require work?",
		new List<Response> {
			new Response(ResponseType.Money, "You explain that unused energy can be sold. This could help him pay for the solar panels!", 
			"Hmm. The cost isn't an issue. I was more interested in the process of selling, but maybe we can discuss it more some other time.", false),
			new Response(ResponseType.Technology, "You tell him solar energy can be sold when the meter runs backwards.  In the future, you may be able to sell green energy for more than power from fossil fuels.", 
			"I love the ease. It will be interesting when green energy is worth more, but I just might invest in solar panels now!", true)
		} ));
	}

	public void SetNeighborEM(Neighbor neighbor) {
		discussions.Add(new Discussion(neighbor, "introduction", "I wasn't expecting a visitor today! My name's Gerald. You're the new Smart Grid Manager, aren't you? What's a Smart Grid, anyways?",
		new List<Response> { 
			new Response(ResponseType.Greet, "You introduce yourself to Gerald. Then you explain that smart grids are power grids with higher tech. A smart grid can detect and react to changes in energy use.", "Hmm. That sounds much less complicated than I thought. I'd love to learn more about what makes them so smart sometime! It was nice to meet you, neighbor.")
		} ));

		discussions.Add(new Discussion(neighbor, "offPeak", "Nice to see you again, neighbor. I just got a SmartTech washing machine installed! The folks told me I can somehow save money by using it during off-peak hours. Do you know what that means?",
		new List<Response> { 
			new Response(ResponseType.Money, "You explain that the price of energy changes with demand. You can save money by using appliances when other people aren't.", 
			"Hmmm sounds great! I'll start doing my laundry in the early mornings, then.  Before all the younguns wake up!", true),
			new Response(ResponseType.Technology, "You explain that power prices change based on how many people are using it. You can set appliances to run when prices are low.", 
			"Listen, I don't really understand technology all that well. To be honest, I don't think I can figure out how to do that. Maybe you can stop by some other time and try to explain it to me again?", false)
		} ));

		discussions.Add(new Discussion(neighbor, "dryerRack", "Nice to see you again, neighbor. My old pal Bernie was telling me he dries his clothes on a rack instead of using an electronic dryer. Is that a good idea?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that drying racks are a great way to reduce energy use. All energy use has an impact on the Earth.", 
			"Well that sounds easy! I never liked all the noisy racket my old dryer made anyways. Thanks for the tip.", true),
			new Response(ResponseType.Money, "You explain that drying racks can save money. They don't use electricity to dry clothes!", 
			"Well, I'm always looking for ways to save money! My grandkids will need every penny to pay for college. Thanks for the advice, neighbor.", true),
			new Response(ResponseType.Technology, "You explain that dryer racks are the latest trend. You tell him that the Turbo Dryinator 4000 is popular. It can dry twice as many clothes as a dryer.", 
			"A turbo dryinator? Wouldn't an old clothesline do the same thing? Maybe I just don't get it. I think I'll stick with my trusty old dryer.", false)
		} ));

		discussions.Add(new Discussion(neighbor, "turnLightsOff", "Thanks for stopping by! The power company sent me a letter in the mail telling me I shouldn't leave my lights on all night. Why's it matter if I'm leaving ‘em on?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that wasting energy can hurt the Earth. That's why you should always turn lights off when you don't need them.", 
			"I’m not sure about that. A little extra light has never hurt anyone. Thanks, though.  I have to go cash a check, but I hope to see you again soon.", false),
			new Response(ResponseType.Money, "You tell him that his lights cost him money when they're on. He can save money by turning them off when they're not in use.", 
			"Well, you know I'm always looking for ways to save money! I guess there isn't any reason I should be leaving them on at night anyways. Thanks for helping me save money!", true),
			new Response(ResponseType.Technology, "You inform Gerald that he could connect his lights to a smart outlet. The outlet could turn lights off at midnight every night.", 
			"I think that's a bit too complicated for me. What if the lights turn off and I'm stuck in my reading chair in the dark? I like leaving them on anyways, but thanks for stopping by.", false)
		} ));

		discussions.Add(new Discussion(neighbor, "unplug", "Howdy, neighbor. Good to see you again. Every winter I vacation somewhere warmer. My nephew told me I need to unplug my appliances to stop them from using power. Is that true? I can’t imagine bending over to unplug them if I can just turn them off.",
		new List<Response> {
			new Response(ResponseType.Environment, "You inform Gerald that most appliances still use power when they're off. When you won't be using one for a long time, you can unplug them to save energy.", 
			"Now that I think about it, my TV has a little red light on even when it's turned off.  Of course, I hear that those little LEDs don’t use much energy.  I’d rather not break my back unplugging something.", false),
			new Response(ResponseType.Money, "You inform Gerald that most appliances use power even when they're 'off'. If he won't be using one for a long time, he can save money by unplugging them.", 
			"Now that I think about it, my TV has a little red light that's always on. Of course, I hear that those little LEDs don’t use much energy.  I’d rather not break my back unplugging something.", false),
			new Response(ResponseType.Technology, "You inform him that appliances use power even when they're 'off'. If he plugged them into a smart outlet, he could turn them off from his phone.", 
			"You mean I don’t even have to bend over?  Some technology really is amazing.", true)
		} ));

		discussions.Add(new Discussion(neighbor, "solarSell", "Hey there, youngun. My son Gerald Jr just got some solar panels installed. He was talking about how good they are for him, but I didn't quite understand it all. Can you explain why I might want solar panels?",
		new List<Response> {
			new Response(ResponseType.Environment, "You explain that solar panels allow people to capture the sun’s energy. As a result, you use less fossil fuel and reduce pollution. This can help keep the Earth clean for many generations.", 
			"Ahh I see. Well if it's good for my grandchildren, I think everyone should have some of them solar panels. Thanks for explaining it so clearly.", true),
			new Response(ResponseType.Money, "You explain that solar panels reduce the amount of energy you buy. If you generate more power than you use in a month, the grid will pay you!", 
			"Huh who woulda thought you could make money from the sun? That's very insightful!  Thanks for explaining it to me.", true),
			new Response(ResponseType.Technology, "You explain to Gerald Sr that solar panels generate renewable energy that is stored in our microgrid and in exchange you receive energy credits that are spent whenever you normally use energy. If you have leftover energy credits at the end of the month you can sell them back to the grid for a profit.", 
			"You sound just like Junior. I have no idea what half those words mean. Listen I've gotta run but maybe we could talk about this some other time?", false)
		} ));
	}

	public Discussion GetDiscussion(string key) {
		for(int i = 0; i < discussions.Count; i++) {
			if(discussions[i].key == key) {
				return discussions[i];
			}
		}

		Debug.Log("That key doesn't connect with any discussions.");
		return null;
	}

	public Discussion GetDiscussion(string key, Neighbor neighbor) {
		for(int i = 0; i < discussions.Count; i++) {
			if(discussions[i].key == key && discussions[i].neighbor == neighbor) {
				return discussions[i];
			}
		}

		Debug.Log("That key doesn't connect with any discussions.");
		return null;
	}

	public Discussion GetRandomDiscussion(Neighbor neighbor) {
		List<Discussion> possibilities = new List<Discussion>();
		for(int i = 0; i < discussions.Count; i++) {
			if(discussions[i].neighbor == neighbor) {
				if(discussions[i].key != "introduction") {
					possibilities.Add(discussions[i]);
				}
			}
		}

		if(possibilities.Count > 0) {
			return possibilities[Random.Range(0, possibilities.Count)];
		} else {
			Debug.Log("All out of decisions");
			
			return null;
		}
	}

	public void RemoveDiscussion(Discussion discussion) {
		discussions.Remove(discussion);
	}
}
