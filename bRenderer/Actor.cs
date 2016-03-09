using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/** @brief An Actor is any object that can be placed into a level. Actors are a generic Class that supports 3D transformations such as translation, rotation, and scale.
*	@author Benjamin Buergisser
*/
public abstract class Actor
{
    /* Constructors */

    /**	@brief Constructor loading standard values for position, orientation and projection
	*/
    public Actor()
    {
        _scale = new Vector3(1f);
    }

    /**	@brief Constructor 
	*	@param[in] position Position of the actor
	*	@param[in] rotationAxes Rotation axes of the actor
    *	@param[in] scale Scale of the actor
	*/
    public Actor(Vector3 position, Vector3 rotationAxes, Vector3 scale)
    {
        _position = position;
        _rotationAxes = rotationAxes;
        _scale = scale;
    }

    /* Public functions */

    /**	@brief Moves the actor forward with a certain speed (based on previous position)
	*	@param[in] speed The velocity of the movement
	*/
    public void moveForward(float speed)
    {
        _position += speed * getForward();
    }

    /**	@brief Moves the actor to the right with a certain speed (based on previous position)
	*	@param[in] speed The velocity of the movement
	*/
    public void moveSideward(float speed)
    {
        _position -= speed * getRight();
    }

    /**	@brief Moves the actor upwards with a certain speed (based on previous position)
	*	@param[in] speed The velocity of the movement
	*/
    public void moveUpward(float speed)
    {
        _position -= speed * getUp();
    }

    /**	@brief Rotates the actor based on previous orientation
	*	@param[in] rotationX The rotation around the x axis in radians
	*	@param[in] rotationY The rotation around the y axis in radians
	*	@param[in] rotationZ The rotation around the z axis in radians
	*/
    public void rotate(float rotationX, float rotationY, float rotationZ)
    {
        _rotationAxes.X += rotationX;
        _rotationAxes.Y += rotationY;
        _rotationAxes.Z += rotationZ;
    }

    /**	@brief Sets the position of the actor
	*	@param[in] position Position of the actor
	*/
    public void setPosition(Vector3 position) { _position = position; }

    /**	@brief Sets the rotation matrix of the actor
	*	@param[in] rotationAxes Rotation axes of the actor
	*/
    public void setRotation(Vector3 rotationAxes) { _rotationAxes = rotationAxes; }

    /**	@brief Scales the actor
    *	@param[in] scale
    */
    public void setScale(Vector3 scale)   { _scale = scale; }

    /**	@brief Returns the world matrix of the actor
	*/
    public virtual Matrix getWorldMatrix()
    {
        return Matrix.CreateScale(_scale) * Matrix.CreateTranslation(getPosition()) * getRotation();
    }

    /**	@brief Returns the inverse of the world matrix of the actor
	*
	*	The inverse world matrix may be useful to keep another actor at the actors position 
	*/
    public virtual Matrix getInverseWorldMatrix()
    {
        return getInverseRotation() * Matrix.CreateTranslation(-getPosition()) * Matrix.CreateScale(new Vector3(1f / _scale.X, 1f / _scale.Y, 1f / _scale.Z));
    }

    /**	@brief Returns the position of the actor
	*/
    public Vector3 getPosition() { return _position; }

    /**	@brief Returns the sclae of the actor
	*/
    public Vector3 getScale() { return _scale; }

    /**	@brief Returns the rotation matrix of the actor
	*/
    public Matrix getRotation()
    {
        return Matrix.CreateRotationY(_rotationAxes.Y) * Matrix.CreateRotationX(_rotationAxes.X) * Matrix.CreateRotationZ(_rotationAxes.Z);
    }

    /**	@brief Returns the inverse of the rotation matrix of the actor
	*
	*	The inverse rotation matrix may be useful to keep another actor to face the actor (e.g. a sprite)
	*/
    public Matrix getInverseRotation()
    {
        return Matrix.Transpose(getRotation());
    }

    /**	@brief Returns the inverse of the x-axis of the rotation matrix
	*/
    public Matrix getInverseRotationX()
    {
        return Matrix.Transpose(Matrix.CreateRotationX(_rotationAxes.X));
    }

    /**	@brief Returns the inverse of the y-axis of the rotation matrix
	*/
    public Matrix getInverseRotationY()
    {
        return Matrix.Transpose(Matrix.CreateRotationY(_rotationAxes.Y));
    }

    /**	@brief Returns the inverse of the z-axis of the rotation matrix
	*/
    public Matrix getInverseRotationZ()
    {
        return Matrix.Transpose(Matrix.CreateRotationZ(_rotationAxes.Z));
    }

    /**	@brief Returns the orientation of the actor
	*/
    public Vector3 getForward()
    {
        Matrix r = getRotation();
        return new Vector3(r[0, 2], r[1, 2], r[2, 2]);
    }

    /**	@brief Returns the right vector of the actor
	*/
    public Vector3 getRight()
    {
        Matrix r = getRotation();
        return new Vector3(r[0, 0], r[1, 0], r[2, 0]);
    }

    /**	@brief Returns the up vector of the actor
	*/
    public Vector3 getUp()
    {
        Matrix r = getRotation();
        return new Vector3(r[0, 1], r[1, 1], r[2, 1]);
    }

    /**	@brief Returns true if the actor is able to collide with other objects
    */
    public bool isCollider()
    {
        return _isCollider;
    }

    /**	@brief Defines wether the actor is able to collide with other actors
    *	@param[in] isCollider Set true if the actor is able to collide with other objects
    */
    public void setCollider(bool isCollider)
    {
        _isCollider = isCollider;
    }

    /**	@brief Tests for collision with another actor
    *	@param[in] actor Actor to test against
    */
    public abstract bool isColliding(Actor actor);

    /**	@brief Tests for intersection with a bounding box
    *	@param[in] boundingBox
    */
    public abstract bool doesIntersect(BoundingBox boundingBox);

    /**	@brief Tests for intersection with a bounding sphere
    *	@param[in] boundingSphere
    */
    public abstract bool doesIntersect(BoundingSphere boundingSphere);

    /**	@brief Notifies actor of a collision
    *	@param[in] actor Actor that is colliding with this actor
    */
    public abstract void onCollision(Actor actor);

    /**	@brief Returns the bounding sphere of the actor in object space
    */
    public abstract BoundingSphere getBoundingSphere();

    /**	@brief Returns the bounding box of the actor in object space
    */
    public abstract BoundingBox getBoundingBox();

    /**	@brief Returns the bounding sphere of the actor in world space
*/
    public abstract BoundingSphere getBoundingSphereWorld();

    /**	@brief Returns the bounding box of the actor in world space
    */
    public abstract BoundingBox getBoundingBoxWorld();

    /* Variables */

    private Vector3 _position;
    private Vector3 _rotationAxes;
    private Vector3 _scale;
    private bool _isCollider = true;
}
