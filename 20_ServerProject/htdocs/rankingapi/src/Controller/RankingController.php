<?php
namespace App\Controller;

use App\Controller\AppController;

/**
 * Ranking Controller
 *
 * @property \App\Model\Table\RankingTable $Ranking
 *
 * @method \App\Model\Entity\Ranking[]|\Cake\Datasource\ResultSetInterface paginate($object = null, array $settings = [])
 */
class RankingController extends AppController
{

    /**
     * Index method
     *
     * @return \Cake\Http\Response|void
     */
    public function index()
    {
        $ranking = $this->paginate($this->Ranking);

        $this->set(compact('ranking'));
    }

    /**
     * View method
     *
     * @param string|null $id Ranking id.
     * @return \Cake\Http\Response|void
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function view($id = null)
    {
        $ranking = $this->Ranking->get($id, [
            'contain' => []
        ]);

        $this->set('ranking', $ranking);
    }

    /**
     * Add method
     *
     * @return \Cake\Http\Response|null Redirects on successful add, renders view otherwise.
     */
    public function add()
    {
        $ranking = $this->Ranking->newEntity();
        if ($this->request->is('post')) {
            $ranking = $this->Ranking->patchEntity($ranking, $this->request->getData());
            if ($this->Ranking->save($ranking)) {
                $this->Flash->success(__('The ranking has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The ranking could not be saved. Please, try again.'));
        }
        $this->set(compact('ranking'));
    }

    /**
     * Edit method
     *
     * @param string|null $id Ranking id.
     * @return \Cake\Http\Response|null Redirects on successful edit, renders view otherwise.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function edit($id = null)
    {
        $ranking = $this->Ranking->get($id, [
            'contain' => []
        ]);
        if ($this->request->is(['patch', 'post', 'put'])) {
            $ranking = $this->Ranking->patchEntity($ranking, $this->request->getData());
            if ($this->Ranking->save($ranking)) {
                $this->Flash->success(__('The ranking has been saved.'));

                return $this->redirect(['action' => 'index']);
            }
            $this->Flash->error(__('The ranking could not be saved. Please, try again.'));
        }
        $this->set(compact('ranking'));
    }

    /**
     * Delete method
     *
     * @param string|null $id Ranking id.
     * @return \Cake\Http\Response|null Redirects to index.
     * @throws \Cake\Datasource\Exception\RecordNotFoundException When record not found.
     */
    public function delete($id = null)
    {
        $this->request->allowMethod(['post', 'delete']);
        $ranking = $this->Ranking->get($id);
        if ($this->Ranking->delete($ranking)) {
            $this->Flash->success(__('The ranking has been deleted.'));
        } else {
            $this->Flash->error(__('The ranking could not be deleted. Please, try again.'));
        }

        return $this->redirect(['action' => 'index']);
    }

    public function getRanking(){
		error_log("getRanking()");
		//---------------
		// View???????????????????????????
		// ???????????????????????????????????????????????????(.tpl)?????????????????????
		$this->autoRender = false;

		//---------------
		// POST???????????????????????????
		// API???????????????POST?????????????????????????????????????????? $this->request->data[] ?????????????????????
		// ??????????????? id ????????????????????????????????????????????????????????????????????????
		//$id = $this->request->data['id'];

		//---------------
		// DB????????????????????????????????????????????????
		//[Rankings]????????????????????????????????????
		$query = $this->Ranking->find('all');

		//?????????????????????????????????????????????????????????????????????????????????
//		debug($query);	//??????????????????????????????????????????????????????
//		$query->where(['id' => 1]);			//?????????['id']???[1]??????????????????????????????????????????
		$query->order(['Time' => 'ASC']);		//?????????['id']?????????????????????????????????
//		$query->order(['id' => 'DESC']);	//?????????['id']?????????????????????????????????
		$query->limit(10);					//???????????????3????????????
		
		//????????????????????????array?????????????????????
		$json_array = json_encode($query);

		//---------------
		// $json_array ??????????????????
		echo $json_array;
	}

    public function setRanking()
    {
        $this->autoRender = false;

        //name???time???POST???????????????
        $name = "";
        if(isset($this->request->data['name']))
        {
            $name = $this->request->data['name'];
            error_log($name);
            echo $name;
        }
        $time = "";
        if(isset($this->request->data['time']))
        {
            $time = $this->request->data['time'];
            error_log($time);
            echo $time;
        }

        $data = array('Name' => $name,'Time' => $time,'Date' => date('Y/m/d H:i:s'));

        $Ranking = $this->Ranking->newEntity();
        $Ranking = $this->Ranking->patchEntity($Ranking,$data);

        if($this->Ranking->save($Ranking))
        {
            echo "success";
        }else{
            echo "failed";       
        }
    }
}
