select * from PizzaPicker..PizzaView where IsRandomlygenerated = 1 and eaten = 1 and (topping1 = 'Black Olives' or topping2 = 'Black Olives' or topping3 = 'Black Olives')

select * from toppings
select * from Sauces


-- count of how many times each sauce appears on a pizza; this ist the same for all sauces
select s.Name, count(s.Name) from PizzaView v
inner join sauces s on v.Sauce = s.Name
where v.IsRandomlyGenerated = 1 
group by s.Name


-- count of how many times each distinct topping appears on a pizza; this is the same for all toppings
select Name, sum(total) total
from (
select t.Name, count(t.Name) total
from PizzaView v
left join toppings t on t.Name = v.Topping1
where v.IsRandomlyGenerated = 1
group by t.Name
union all
select t.Name, count(t.Name) total
from PizzaView v
left join toppings t on t.Name = v.Topping2
where v.IsRandomlyGenerated = 1
group by t.Name
union all
select t.Name, count(t.Name) total
from PizzaView v
left join toppings t on t.Name = v.Topping3
where v.IsRandomlyGenerated = 1
group by t.Name
) a
group by Name
order by Name
