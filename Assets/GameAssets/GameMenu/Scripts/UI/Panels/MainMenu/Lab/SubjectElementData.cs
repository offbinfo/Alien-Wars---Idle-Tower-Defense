using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectElementData : BaseUICellData
{
    private Subject_SO subject;

    public Subject_SO Subject_SO => subject;

    public SubjectElementData(Subject_SO subject_SO)
    {
        this.subject = subject_SO;  
    }
}
