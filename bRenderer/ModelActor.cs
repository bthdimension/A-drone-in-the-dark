using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

/** @brief The model actor is a game object that controls a model in the level. 
*	@author Benjamin Buergisser
*/
public class ModelActor : Actor
{
    /* Constructors */

    /**	@brief Constructor loading standard values for position, orientation and projection
    *	@param[in] model
	*/
    public ModelActor(Model model) : base()
    {
        setModel(model);
    }

    /**	@brief Constructor 
    *	@param[in] model
	*	@param[in] position Position of the actor
	*	@param[in] rotationAxes Rotation axes of the actor
    *	@param[in] scale Scale of the actor
	*/
    public ModelActor(Model model, Vector3 position, Vector3 rotationAxes, Vector3 scale)
        : base(position, rotationAxes, scale)
    {
        setModel(model);
    }

    /* Public functions */

    /**	@brief Draws the actor
    *	@param[in] camera 
	*/
    public void draw(CameraActor camera)
    {
        foreach (ModelMesh mesh in _model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = getWorldMatrix();
                effect.View = camera.getViewMatrix();
                effect.Projection = camera.getProjectionMatrix();
            }

            mesh.Draw();
        }
    }

    /**	@brief Sets the model and creates bounding volumes for the actor
    *	@param[in] model 
	*/
    public void setModel(Model model)
    {
        _model = model;
        createBoundingSphere();
    }

    public override bool isColliding(Actor actor)
    {
        if (isCollider())
        {
            if (actor.doesIntersect(getBoundingSphereWorld()))
            {
                if (_model.Meshes.Count > 1)
                    foreach (ModelMesh mesh in _model.Meshes)
                    {
                        if (actor.doesIntersect(boundingSphereToWorld(mesh.BoundingSphere)))
                        {
                            actor.onCollision(this);
                            this.onCollision(actor);
                            return true;
                        }
                    }
                else
                    return true;
            }
        }
        return false;
    }

    public override bool doesIntersect(BoundingBox boundingBox)
    {
        if (getBoundingBoxWorld().Intersects(boundingBox))
        {
            if (_model.Meshes.Count > 1)
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    if (BoundingBox.CreateFromSphere(boundingSphereToWorld(mesh.BoundingSphere)).Intersects(boundingBox))
                        return true;
                }
            else
                return true;
        }
        return false;
    }

    public override bool doesIntersect(BoundingSphere boundingSphere)
    {
        if(getBoundingSphereWorld().Intersects(boundingSphere))
        {
            if (_model.Meshes.Count > 1)
                foreach (ModelMesh mesh in _model.Meshes)
                {
                    if (boundingSphereToWorld(mesh.BoundingSphere).Intersects(boundingSphere))
                        return true;
                }
            else
                return true;
        }
        return false;
    }

    public override void onCollision(Actor actor)
    {
        // ToDo
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

    /* Private Functions */

    private void createBoundingSphere()
    {
        if (_model != null)
        {
            _boundingSphere = new BoundingSphere();
            foreach (ModelMesh mesh in _model.Meshes)
            {
                if (_boundingSphere.Radius == 0)
                    _boundingSphere = mesh.BoundingSphere;
                else
                    _boundingSphere = BoundingSphere.CreateMerged(_boundingSphere, mesh.BoundingSphere);
            }
        }
    }

    private BoundingSphere boundingSphereToWorld(BoundingSphere boundingSphere)
    {
        BoundingSphere bs = new BoundingSphere(boundingSphere.Center, boundingSphere.Radius);
        bs.Center = getPosition();
        bs.Radius *= Util.getMaxAbsVectorValue(getScale());
        return bs;
    }

    /* Variables */

    private Model _model;
    private BoundingSphere _boundingSphere;

}
