using UnityEngine;
using System.Collections;

namespace Plugins.RootMotion {

	/// <summary>
	/// Helper methods for dealing with Quaternions.
	/// </summary>
	public static class QuaTools {

        /// <summary>
        /// Returns yaw angle (-180 - 180) of 'forward' vector relative to rotation space defined by spaceForward and spaceUp axes.
        /// </summary>
        public static float GetYaw(Quaternion space, Vector3 forward)
        {
            Vector3 dirLocal = Quaternion.Inverse(space) * forward;
			if (dirLocal.x == 0f && dirLocal.z == 0f) return 0f;
			if (float.IsInfinity(dirLocal.x) || float.IsInfinity(dirLocal.z)) return 0;

			return Mathf.Atan2(dirLocal.x, dirLocal.z) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns pitch angle (-90 - 90) of 'forward' vector relative to rotation space defined by spaceForward and spaceUp axes.
        /// </summary>
        public static float GetPitch(Quaternion space, Vector3 forward)
        {
            forward = forward.normalized;
            Vector3 dirLocal = Quaternion.Inverse(space) * forward;
			if (Mathf.Abs(dirLocal.y) > 1f) dirLocal.Normalize();
            return -Mathf.Asin(dirLocal.y) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns bank angle (-180 - 180) of 'forward' and 'up' vectors relative to rotation space defined by spaceForward and spaceUp axes.
        /// </summary>
        public static float GetBank(Quaternion space, Vector3 forward, Vector3 up)
        {
            Vector3 spaceUp = space * Vector3.up;

            Quaternion invSpace = Quaternion.Inverse(space);
            forward = invSpace * forward;
            up = invSpace * up;

            Quaternion q = Quaternion.Inverse(Quaternion.LookRotation(spaceUp, forward));
            up = q * up;
            float result = Mathf.Atan2(up.x, up.z) * Mathf.Rad2Deg;
			return Mathf.Clamp(result, -180f, 180f);
        }

		/// <summary>
		/// Returns yaw angle (-180 - 180) of 'forward' vector relative to rotation space defined by spaceForward and spaceUp axes.
		/// </summary>
		public static float GetYaw(Quaternion space, Quaternion rotation)
		{
			Vector3 dirLocal = Quaternion.Inverse(space) * (rotation * Vector3.forward);
			if (dirLocal.x == 0f && dirLocal.z == 0f) return 0f;
			if (float.IsInfinity(dirLocal.x) || float.IsInfinity(dirLocal.z)) return 0;
			return Mathf.Atan2(dirLocal.x, dirLocal.z) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns pitch angle (-90 - 90) of 'forward' vector relative to rotation space defined by spaceForward and spaceUp axes.
		/// </summary>
		public static float GetPitch(Quaternion space, Quaternion rotation)
		{
			Vector3 dirLocal = Quaternion.Inverse(space) * (rotation * Vector3.forward);
			if (Mathf.Abs(dirLocal.y) > 1f) dirLocal.Normalize();
			return -Mathf.Asin(dirLocal.y) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Returns bank angle (-180 - 180) of 'forward' and 'up' vectors relative to rotation space defined by spaceForward and spaceUp axes.
		/// </summary>
		public static float GetBank(Quaternion space, Quaternion rotation)
        {
            Vector3 spaceUp = space * Vector3.up;
            
            Quaternion invSpace = Quaternion.Inverse(space);
            Vector3 forward = invSpace * (rotation * Vector3.forward);
            Vector3 up = invSpace * (rotation * Vector3.up);

            Quaternion q = Quaternion.Inverse(Quaternion.LookRotation(spaceUp, forward));
            up = q * up;
			float result = Mathf.Atan2(up.x, up.z) * Mathf.Rad2Deg;
			return Mathf.Clamp(result, -180f, 180f);
		}

        /// <summary>
        /// Optimized Quaternion.Lerp
        /// </summary>
        public static Quaternion Lerp(Quaternion fromRotation, Quaternion toRotation, float weight) {
			if (weight <= 0f) return fromRotation;
			if (weight >= 1f) return toRotation;

			return Quaternion.Lerp(fromRotation, toRotation, weight);
		}

		/// <summary>
		/// Optimized Quaternion.Slerp
		/// </summary>
		public static Quaternion Slerp(Quaternion fromRotation, Quaternion toRotation, float weight) {
			if (weight <= 0f) return fromRotation;
			if (weight >= 1f) return toRotation;

			return Quaternion.Slerp(fromRotation, toRotation, weight);
		}

		/// <summary>
		/// Returns the rotation from identity Quaternion to "q", interpolated linearily by "weight".
		/// </summary>
		public static Quaternion LinearBlend(Quaternion q, float weight) {
			if (weight <= 0f) return Quaternion.identity;
			if (weight >= 1f) return q;
			return Quaternion.Lerp(Quaternion.identity, q, weight);
		}

		/// <summary>
		/// Returns the rotation from identity Quaternion to "q", interpolated spherically by "weight".
		/// </summary>
		public static Quaternion SphericalBlend(Quaternion q, float weight) {
			if (weight <= 0f) return Quaternion.identity;
			if (weight >= 1f) return q;
			return Quaternion.Slerp(Quaternion.identity, q, weight);
		}

		/// <summary>
		/// Creates a FromToRotation, but makes sure its axis remains fixed near to the Quaternion singularity point.
		/// </summary>
		/// <returns>
		/// The from to rotation around an axis.
		/// </returns>
		/// <param name='fromDirection'>
		/// From direction.
		/// </param>
		/// <param name='toDirection'>
		/// To direction.
		/// </param>
		/// <param name='axis'>
		/// Axis. Should be normalized before passing into this method.
		/// </param>
		public static Quaternion FromToAroundAxis(Vector3 fromDirection, Vector3 toDirection, Vector3 axis) {
			Quaternion fromTo = Quaternion.FromToRotation(fromDirection, toDirection);
			
			float angle = 0;
			Vector3 freeAxis = Vector3.zero;
			
			fromTo.ToAngleAxis(out angle, out freeAxis);
			
			float dot = Vector3.Dot(freeAxis, axis);
			if (dot < 0) angle = -angle;
			
			return Quaternion.AngleAxis(angle, axis);
		}
		
		/// <summary>
		/// Gets the rotation that can be used to convert a rotation from one axis space to another.
		/// </summary>
		public static Quaternion RotationToLocalSpace(Quaternion space, Quaternion rotation) {
			return Quaternion.Inverse(Quaternion.Inverse(space) * rotation);
		}

		/// <summary>
		/// Gets the Quaternion from rotation "from" to rotation "to".
		/// </summary>
		public static Quaternion FromToRotation(Quaternion from, Quaternion to) {
			if (to == from) return Quaternion.identity;

			return to * Quaternion.Inverse(from);
		}


		/// <summary>
		/// Gets the closest direction axis to a vector. Input vector must be normalized!
		/// </summary>
		public static Vector3 GetAxis(Vector3 v) {
			Vector3 closest = Vector3.right;
			bool neg = false;

			float x = Vector3.Dot(v, Vector3.right);
			float maxAbsDot = Mathf.Abs(x);
			if (x < 0f) neg = true;

			float y = Vector3.Dot(v, Vector3.up);
			float absDot = Mathf.Abs(y);
			if (absDot > maxAbsDot) {
				maxAbsDot = absDot;
				closest = Vector3.up;
				neg = y < 0f;
			}

			float z = Vector3.Dot(v, Vector3.forward);
			absDot = Mathf.Abs(z);
			if (absDot > maxAbsDot) {
				closest = Vector3.forward;
				neg = z < 0f;
			}

			if (neg) closest = -closest;
			return closest;
 		}

		/// <summary>
		/// Clamps the rotation similar to V3Tools.ClampDirection.
		/// </summary>
		public static Quaternion ClampRotation(Quaternion rotation, float clampWeight, int clampSmoothing) {
			if (clampWeight >= 1f) return Quaternion.identity;
			if (clampWeight <= 0f) return rotation;

			float angle = Quaternion.Angle(Quaternion.identity, rotation);
			float dot = 1f - (angle / 180f);
			float targetClampMlp = Mathf.Clamp(1f - ((clampWeight - dot) / (1f - dot)), 0f, 1f);
			float clampMlp = Mathf.Clamp(dot / clampWeight, 0f, 1f);
			
			// Sine smoothing iterations
			for (int i = 0; i < clampSmoothing; i++) {
				float sinF = clampMlp * Mathf.PI * 0.5f;
				clampMlp = Mathf.Sin(sinF);
			}
			
			return Quaternion.Slerp(Quaternion.identity, rotation, clampMlp * targetClampMlp);
		}

		/// <summary>
		/// Clamps an angular value.
		/// </summary>
		public static float ClampAngle(float angle, float clampWeight, int clampSmoothing) {
			if (clampWeight >= 1f) return 0f;
			if (clampWeight <= 0f) return angle;
			
			float dot = 1f - (Mathf.Abs(angle) / 180f);
			float targetClampMlp = Mathf.Clamp(1f - ((clampWeight - dot) / (1f - dot)), 0f, 1f);
			float clampMlp = Mathf.Clamp(dot / clampWeight, 0f, 1f);
			
			// Sine smoothing iterations
			for (int i = 0; i < clampSmoothing; i++) {
				float sinF = clampMlp * Mathf.PI * 0.5f;
				clampMlp = Mathf.Sin(sinF);
			}
			
			return Mathf.Lerp(0f, angle, clampMlp * targetClampMlp);
		}

		/// <summary>
		/// Used for matching the rotations of objects that have different orientations.
		/// </summary>
		public static Quaternion MatchRotation(Quaternion targetRotation, Vector3 targetAxis1, Vector3 targetAxis2, Vector3 axis1, Vector3 axis2) {
			Quaternion f = Quaternion.LookRotation(axis1, axis2);
			Quaternion fTarget = Quaternion.LookRotation(targetAxis1, targetAxis2);

			Quaternion d = targetRotation * fTarget;
			return d * Quaternion.Inverse(f);
		}

        /// <summary>
        /// Converts an Euler rotation from 0 to 360 representation to -180 to 180.
        /// </summary>
        public static Vector3 ToBiPolar(Vector3 euler)
        {
            return new Vector3(ToBiPolar(euler.x), ToBiPolar(euler.y), ToBiPolar(euler.z));
        }

        /// <summary>
        /// Converts an angular value from 0 to 360 representation to -180 to 180.
        /// </summary>
        public static float ToBiPolar(float angle)
        {
            angle = angle % 360f;
            if (angle >= 180f) return angle - 360f;
            if (angle <= -180f) return angle + 360f;
            return angle;
        }

        /// <summary>
        /// Mirrors a Quaternion on the YZ plane in provided rotation space.
        /// </summary>
        public static Quaternion MirrorYZ(Quaternion r, Quaternion space)
        {
            r = Quaternion.Inverse(space) * r;
            Vector3 forward = r * Vector3.forward;
            Vector3 up = r * Vector3.up;

            forward.x *= -1;
            up.x *= -1;

            return space * Quaternion.LookRotation(forward, up);
        }

        /// <summary>
        /// Mirrors a Quaternion on the world space YZ plane.
        /// </summary>
        public static Quaternion MirrorYZ(Quaternion r)
        {
            Vector3 forward = r * Vector3.forward;
            Vector3 up = r * Vector3.up;

            forward.x *= -1;
            up.x *= -1;

            return Quaternion.LookRotation(forward, up);
        }
    }
}
