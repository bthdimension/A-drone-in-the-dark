using Microsoft.Xna.Framework;
using System;

/** @brief The camera actor defines the view and projection matrices of a scene. 
*	@author Benjamin Buergisser
*/
public class CameraActor : Actor
{
    /* Constructors */

    /**	@brief Constructor loading standard values for position, orientation and projection
	*/
    public CameraActor() : base()
	{
        _boundingSphere = new BoundingSphere();
	}

    /**	@brief Constructor loading standard values for projection
	*	@param[in] position Position of the camera
	*	@param[in] rotationAxes Rotation axes of the camera
	*/
    public CameraActor(Vector3 position, Vector3 rotationAxes) : base(position, rotationAxes, new Vector3(1f))
    {
        _boundingSphere = new BoundingSphere();
    }

    /**	@brief Constructor loading standard values for position and orientation
	*	@param[in] fov Field of view
	*	@param[in] aspect Aspect ratio
	*	@param[in] near Near clipping plane
	*	@param[in] far Far clipping plane
	*/
    public CameraActor(float fov, float aspect, float near, float far)
    {
        _fov = fov;
        _aspect = aspect;
        _near = near;
        _far = far;

        _boundingSphere = new BoundingSphere();
    }

    /**	@brief Constructor
	*	@param[in] position Position of the camera
	*	@param[in] rotationAxes Rotation axes of the camera
	*	@param[in] fov Field of view
	*	@param[in] aspect Aspect ratio
	*	@param[in] near Near clipping plane
	*	@param[in] far Far clipping plane
	*/
    public CameraActor(Vector3 position, Vector3 rotationAxes, float fov, float aspect, float near, float far)
        : base(position, rotationAxes, new Vector3(0f))
    {
        _fov = fov;
        _aspect = aspect;
        _near = near;
        _far = far;
    }

    /* Public Functions */

	/**	@brief Sets field of view
	*	@param[in] fov Field of view
	*/
    public void setFieldOfView(float fov) { _fov = fov; }

	/**	@brief Sets aspect ratio
	*	@param[in] aspect Aspect ratio
	*/
    public void setAspectRatio(float aspect) { _aspect = aspect; }

	/**	@brief Sets near clipping plane
	*	@param[in] near Near clipping plane
	*/
    public void setNearClippingPlane(float near) { _near = near; }

	/**	@brief Sets far clipping plane
	*	@param[in] far Far clipping plane
	*/
    public void setFarClippingPlane(float far) { _far = far; }
    
	/**	@brief Returns the view matrix of the camera
	*/
    public Matrix getViewMatrix()
    {
        return base.getWorldMatrix();
    }

	/**	@brief Returns the inverse of the view matrix of the camera
	*
	*	The inverse view matrix may be useful to keep an object at the cameras position 
	*/
    public Matrix getInverseViewMatrix()
    {
        return base.getInverseWorldMatrix();
    }

	/**	@brief Returns the view projection of the camera
	*/
    public Matrix getProjectionMatrix()
    {
        return createPerspective(_fov, _aspect, _near, _far);
    }

    /**	@brief Returns the world matrix of the actor
    */
    public override Matrix getWorldMatrix()
    {
        return base.getInverseWorldMatrix();
    }

    /**	@brief Returns the inverse of the world matrix of the actor
	*
	*	The inverse world matrix may be useful to keep another actor at the actors position 
	*/
    public override Matrix getInverseWorldMatrix()
    {
        return base.getWorldMatrix();
    }

    /* Collision */

    public override bool isColliding(Actor actor)
    {
        if (isCollider())
        {
            if (actor.doesIntersect(boundingSphereToWorld(_boundingSphere)))
            {
                actor.onCollision(this);
                this.onCollision(actor);
                return true;
            }
        }
        return false;
    }

    public override bool doesIntersect(BoundingBox boundingBox)
    {
        if (getBoundingBoxWorld().Intersects(boundingBox))
        {
            return true;
        }
        return false;
    }

    public override bool doesIntersect(BoundingSphere boundingSphere)
    {
        if (getBoundingSphereWorld().Intersects(boundingSphere))
        {
            return true;
        }
        return false;
    }

    public override void onCollision(Actor actor)
    {
        Console.WriteLine("camera collided");
    }

    public override BoundingSphere getBoundingSphere()
    {
        return _boundingSphere;
    }

    public override BoundingBox getBoundingBox()
    {
        return BoundingBox.CreateFromSphere(getBoundingSphere());
    }

    public override BoundingSphere getBoundingSphereWorld()
    {
        return boundingSphereToWorld(_boundingSphere);
    }

    public override BoundingBox getBoundingBoxWorld()
    {
        return BoundingBox.CreateFromSphere(getBoundingSphereWorld());
    }

	/* Static Functions */

	/**	@brief Create a 3D perspective
	*	@param[in] fov Field of view
	*	@param[in] aspect Aspect ratio
	*	@param[in] near Near clipping plane
	*	@param[in] far Far clipping plane
	*/
    public static Matrix createPerspective(float fov, float aspect, float near, float far)
    {
        Matrix perspective = Matrix.Identity;

	    for (int i = 0; i<3; i++) {
		    for (int j = 0; j<3; j++) {
			    perspective[i, j] = 0.0f;
		    }
	    }

        float angle = MathHelper.ToRadians(fov);
        float f = (float)(1.0 / Math.Tan((double)(angle * 0.5)));

	    perspective[0, 0] = f / aspect;
	    perspective[1, 1] = f;
	    perspective[2, 2] = (far + near) / (near - far);
	    perspective[2, 3] = -1.0f;
	    perspective[3, 2] = (2.0f * far*near) / (near - far);


	    return perspective;
    }

    /* Private Functions */

    private BoundingSphere boundingSphereToWorld(BoundingSphere boundingSphere)
    {
        BoundingSphere bs = new BoundingSphere(boundingSphere.Center, boundingSphere.Radius);
        bs.Center = -getPosition();
        return bs;
    }


	/* Variables */

    private float _fov      = 60f;
    private float _aspect   = 16f/9f;
    private float _near     = -1f;
    private float _far      = 1f;

    private BoundingSphere _boundingSphere;
}
