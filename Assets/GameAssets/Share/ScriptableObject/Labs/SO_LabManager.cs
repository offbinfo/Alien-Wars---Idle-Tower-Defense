using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LabManager", menuName = "Data/Labs/LabManager", order = 0)]
public class SO_LabManager : SerializedScriptableObject
{
    public Dictionary<SubjectType, List<Subject_SO>> subjectDicts = new();
    public List<Subject_SO> subjects = new();

    public List<Subject_SO> GetAllSubjectByType(SubjectType subjectType)
    {
        if (subjectDicts.ContainsKey(subjectType))
        {
            return subjectDicts[subjectType];
        }
        return null;
    }

    public List<Subject_SO> GetAllSubject()
    {
        return subjectDicts.Values.SelectMany(list => list).ToList();
    }

    public Subject_SO GetSingleSubjectById(IdSubjectType idSubjectType)
    {
        return subjects.FirstOrDefault(subject => subject.id == idSubjectType);
    }
}
