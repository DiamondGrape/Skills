using ScriptableObjectMultiSelectDropdown;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : BaseSkillData
{
	/// <summary>
	///	Is the units level more or equal to the level needed?
	/// </summary>
	/// <param name="unit">Target of the skill.</param>
	protected virtual bool CheckLevel(Unit unit)
	{
		return unit.actorLevel >= LevelNeeded;
	}

	/// <summary>
	///	Is the skill already on the unit?
	/// </summary>
	/// <param name="unit">Unit skill list.</param>
	protected virtual bool CheckSkillsList(Unit unit)
	{
		/*
		List<BaseSkill>.Enumerator skills = unit.skills.GetEnumerator();
		while(skills.MoveNext())
		{
			var currSkill = skills.Current;
			
			if(currSkill.BaseName == this.BaseName)
			{

				return true;

			}
		}
		return false;
		*/

		foreach(BaseSkill baseSkill in unit.skills)
		{
			if(baseSkill.BaseName == this.BaseName)
			{
				Debug.Log($"{name} is on the current Unit.");
				return true;
			}
		}
		return false;
	}

	/// <summary>
	///	The skill will do this on being added to the skill bar.
	/// </summary>
	/// <param name="unit">Unit the skill will be added to.</param>
	public virtual void AddSKill(Unit unit)
	{
		if (CheckLevel(unit) && !CheckSkillsList(unit))
		{
			unit.skills.Add(this);
			Perform(unit);
		}
	}

	/// <summary>
	///	The skill will do this on being added to the skill bar.
	/// </summary>
	/// <param name="unit">Unit the thing will be perfromed on.</param>
	public virtual void Perform(Unit unit)
	{

	}

	/// <summary>
	///	Remove the skill.
	/// </summary>
	/// <param name="unit">Unit the skill will be removed from.</param>
	public virtual void Remove(object unit)
	{
		Unit curUnit = (Unit)unit;
		
		if (CheckSkillsList(curUnit))
		{
			StopPerforming(curUnit);
			curUnit.skills.Remove(this);

		}
	}

	/// <summary>
	///	 Stop performing the function of this skill.
	/// </summary>
	/// <param name="unit">Unit the skill willl not be performing for.</param>
	public virtual void StopPerforming(Unit unit)
	{
		Debug.Log($"{BaseName}: Is no longer performing");
	}

	/// <summary>
	///	Does the unit meet the requirements?
	/// </summary>
	/// <param name="unit"> Unit required.</param>
	/// <param name="requiredVocation"> The required vocation for the current skill.</param>
	public virtual bool CheckRequirements(Unit unit, ScriptableObjectReference requiredVocation)
	{
		bool vocationRequirement = VocationRequirements(unit, requiredVocation);
		
		if(vocationRequirement)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	///	Does the unit meet the vocation requirements?
	/// </summary>
	/// <param name="unit"> Unit required.</param>
	/// <param name="requiredVocation"> The required vocation for the current skill.</param>
	private static bool VocationRequirements(Unit unit, ScriptableObjectReference requiredVocation)
	{
		VocationBaseData primaryVocation = unit.primaryVocation;
		VocationBaseData secondaryVocation = unit.secondaryVocation;

		bool primaryVocationMet = CheckVocation(unit, requiredVocation, primaryVocation);
		bool secondaryVocationMet = CheckVocation(unit, requiredVocation, secondaryVocation);

		int requiredVocationLength = requiredVocation.values.Length;

		switch (requiredVocationLength)
		{
			// if there is only one requirement if either of the primary vocations meet the requirement then return true.
			case 1:
				if (primaryVocationMet || secondaryVocationMet)
				{
					return true;
				}

				break;

			default:
				if (primaryVocationMet && secondaryVocationMet)
				{
					return true;
				}
				break;
		}
		Debug.Log($"{unit.name} has not met the requirements");
		return false;
	}

	/// <summary>
	///	Does one of the units vocations equal to one of the required vocations?
	/// </summary>
	/// <param name="unit"> Unit required.</param>
	/// <param name="requiredVocation"> The required vocation for the current skill.</param>
	/// <param name="unitVocation"> Units vocation being checked.</param>
	private static bool CheckVocation(Unit unit,ScriptableObjectReference requiredVocation, VocationBaseData unitVocation)
	{

		string vocationName = unitVocation.Name;
		int requiredVocationLength = requiredVocation.values.Length;

		for (int i = 0; i < requiredVocationLength; i++)
		{
			string requiredVocationName = requiredVocation.values[i].name;

			if (vocationName == requiredVocationName)
			{
				Debug.Log($"{unit.name} has met the requirements vocation requirement:{requiredVocationName}.");
				return true;
			}
			continue;
		}
		return false;
	}
}
