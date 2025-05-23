﻿// author:KIPKIPS
// date:2023.04.15 04:20
// describe:Image绑定类
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

using UnityEngine;

namespace Framework.Core.Manager.UI
{
    [BinderComponent(typeof(LImage))]
    public class LImageBinder : BaseBinder
    {
        [BinderField(typeof(LImage))]
        public enum AttributeType
        {
            sprite = 10000 + LinkerType.String,
            color = 20000 + LinkerType.Color,
            enabled = 30000 + LinkerType.Boolean,
            fillAmount = 40000 + LinkerType.Single,
        }

        public override void SetString(Object mono, int linkerType, string value)
        {
            if (mono == null) return;
            var target = mono as LImage;
            if (target == null)
            {
                return;
            }

            switch ((AttributeType)linkerType)
            {
                case AttributeType.sprite:
                    // target.sprite = value;
                    break;
            }
        }
        public override void SetSingle(Object mono, int linkerType, float value) {
            if (mono == null) return;
            var target = mono as LImage;
            if (target == null)
            {
                return;
            }

            switch ((AttributeType)linkerType)
            {
                case AttributeType.fillAmount:
                    target.fillAmount = value;
                    break;
            }
        }
        public override void SetColor(Object mono, int linkerType, Color value)
        {
            if (mono == null) return;
            var target = mono as LImage;
            if (target == null)
            {
                return;
            }

            switch ((AttributeType)linkerType)
            {
                case AttributeType.color:
                    target.color = value;
                    break;
            }
        }

        public override void SetBoolean(Object mono, int linkerType, bool value)
        {
            if (mono == null) return;
            var target = mono as LImage;
            if (target == null)
            {
                return;
            }

            switch ((AttributeType)linkerType)
            {
                case AttributeType.enabled:
                    target.enabled = value;
                    break;
            }
        }
    }
}