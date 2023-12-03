
using UnityEngine;

namespace Utils
{
    public static class QuaternionUtil {
	
        public static Quaternion AngVelToDeriv(Quaternion Current, Vector3 AngVel) {
            var Spin = new Quaternion(AngVel.x, AngVel.y, AngVel.z, 0f);
            var Result = Spin * Current;
            return new Quaternion(0.5f * Result.x, 0.5f * Result.y, 0.5f * Result.z, 0.5f * Result.w);
        } 

        public static Vector3 DerivToAngVel(Quaternion Current, Quaternion Deriv) {
            var Result = Deriv * Quaternion.Inverse(Current);
            return new Vector3(2f * Result.x, 2f * Result.y, 2f * Result.z);
        }

        public static Quaternion IntegrateRotation(Quaternion Rotation, Vector3 AngularVelocity, float DeltaTime) {
            if (DeltaTime < Mathf.Epsilon) return Rotation;
            var Deriv = AngVelToDeriv(Rotation, AngularVelocity);
            var Pred = new Vector4(
                Rotation.x + Deriv.x * DeltaTime,
                Rotation.y + Deriv.y * DeltaTime,
                Rotation.z + Deriv.z * DeltaTime,
                Rotation.w + Deriv.w * DeltaTime
            ).normalized;
            return new Quaternion(Pred.x, Pred.y, Pred.z, Pred.w);
        }
	
        public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time) {
            if (Time.deltaTime < Mathf.Epsilon) return rot;
            // account for double-cover
            var Dot = Quaternion.Dot(rot, target);
            var Multi = Dot > 0f ? 1f : -1f;
            target.x *= Multi;
            target.y *= Multi;
            target.z *= Multi;
            target.w *= Multi;
            // smooth damp (nlerp approx)
            var Result = new Vector4(
                Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
                Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
                Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
                Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
            ).normalized;
		
            // ensure deriv is tangent
            var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
            deriv.x -= derivError.x;
            deriv.y -= derivError.y;
            deriv.z -= derivError.z;
            deriv.w -= derivError.w;		
		
            return new Quaternion(Result.x, Result.y, Result.z, Result.w);
        }
    }
    
    public static class QuaternionExtensions
    {
        public static Vector4 ToVector4 (this Quaternion quaternion) {
            return new Vector4 (quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }

        public static Quaternion ToQuaternion (this Vector4 vector) {
            return new Quaternion (vector.x, vector.y, vector.z, vector.w);
        }
        
        public static Vector4 SmoothDamp (Vector4 current, Vector4 target, ref Vector4 currentVelocity, float smoothTime) {
            
            
            float x = Mathf.SmoothDamp (current.x, target.x, ref currentVelocity.x, smoothTime);
            float y = Mathf.SmoothDamp (current.y, target.y, ref currentVelocity.y, smoothTime);
            float z = Mathf.SmoothDamp (current.z, target.z, ref currentVelocity.z, smoothTime);
            float w = Mathf.SmoothDamp (current.w, target.w, ref currentVelocity.w, smoothTime);

            return new Vector4 (x, y, z, w);
        }

        public static Quaternion SmoothDamp (Quaternion current, Quaternion target, ref Vector4 currentVelocity, float smoothTime) {
            Vector4 smooth = SmoothDamp (current.ToVector4 (), target.ToVector4 (), ref currentVelocity, smoothTime);
            return smooth.ToQuaternion ();
        }
    }
}